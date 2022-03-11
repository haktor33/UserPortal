using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UserPortal.Context;
using UserPortal.Entities;
using UserPortal.Enums;
using UserPortal.Interfaces;
using UserPortal.Models;

namespace UserPortal.Services
{
    public class KafkaService: IKafkaService
    {
        private readonly ProducerConfig config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        private readonly string topic = "ManagementTopic";
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task ResponseForTopicMesseageType(TopicMessageModel model)
        {
            switch (model.Type)
            {
                case (int)TopicMessageType.RegisterConfirm:
                    var registerApp = JsonSerializer.Deserialize<RegisterApprovementRequest>(model.Data.ToString());
                    var data = await RegisterApprovement(registerApp);
                    model.Data = data;
                    SendToKafka(JsonSerializer.Serialize(model));
                    break;
                case (int)TopicMessageType.UserList:
                    var request = JsonSerializer.Deserialize<PageRequest>(model.Data.ToString());
                    var list = await GetUserList(request);
                    model.Data = list;
                    SendToKafka(JsonSerializer.Serialize(model));
                    break;
                default:
                    break;
            }
        }

        public async Task<List<User>> GetUserList(PageRequest request)
        {
            var skip = request.PageSize * (request.Current - 1);
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var list = await dbContext.Users.Take(request.PageSize).Skip(skip).OrderBy(o => o.Id).ToListAsync();
                return list;
            }
        }

        public async Task<User> RegisterApprovement(RegisterApprovementRequest request)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var user = await dbContext.Users.Where(w => w.Id == request.UserId).FirstOrDefaultAsync();
                if (user == null)
                    throw new Exception("Username or Password not found");
                user.Active = request.Approvement;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
        }

        public Object SendToKafka(string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    return producer.ProduceAsync(topic, new Message<Null, string> { Value = message })
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            }
            return null;
        }

    }
}
