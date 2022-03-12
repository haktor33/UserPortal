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
    public class KafkaService : IKafkaService
    {
        private static readonly ProducerConfig config = new ProducerConfig { BootstrapServers = "kafka:9092" };
        private static readonly string topic = "ManagementTopic";
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<DeliveryResult<Null, string>> ResponseForTopicMesseageType(TopicMessageModel model)
        {
            switch (model.Type)
            {
                case (int)TopicMessageType.RegisterConfirm:
                    var registerReq = JsonSerializer.Deserialize<RegisterApprovementRequest>(model.Data.ToString());
                    var data = await RegisterApprovement(registerReq);
                    model.Data = data;
                    break;
                case (int)TopicMessageType.ChangeStatus:
                    var changeStatusReq = JsonSerializer.Deserialize<ChangeStatusRequest>(model.Data.ToString());
                    var sdata = await ChangeStatus(changeStatusReq);
                    model.Data = sdata;
                    break;
                case (int)TopicMessageType.UserList:
                    var request = JsonSerializer.Deserialize<PageRequest>(model.Data.ToString());
                    var list = await GetUserList(request);
                    model.Data = list;
                    break;
                case (int)TopicMessageType.Test:
                    model.Data = model.Data;
                    break;
                default:
                    break;
            }
            return SendToKafka(JsonSerializer.Serialize(model));
        }

        public async Task<List<User>> GetUserList(PageRequest request)
        {
            if (_scopeFactory == null)
            {
                return new List<User>();
            }
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
                    throw new Exception("User not found");
                if (user.Approvement)
                {
                    user.Approvement = request.Approvement;
                    dbContext.Users.Update(user);
                }
                else
                    dbContext.Users.Remove(user);

                await dbContext.SaveChangesAsync();
                return user;
            }
        }

        public async Task<User> ChangeStatus(ChangeStatusRequest request)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var user = await dbContext.Users.Where(w => w.Id == request.UserId).FirstOrDefaultAsync();
                if (user == null)
                    throw new Exception("User not found");
                user.Active = request.Active;
                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
        }

        public static DeliveryResult<Null, string> SendToKafka(string message)
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
