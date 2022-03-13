using Confluent.Kafka;
using Grpc.Core;
using Management.Enums;
using Management.Models;
using System.Text.Json;

namespace Management.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ProducerConfig config = new ProducerConfig { BootstrapServers = Utils.GetKafkaConfigValue() };
        private readonly string topic = "UserPortalTopic";
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<DefaultReply> RegisterConfirmation(RegisterApprovementRequest request, ServerCallContext context)
        {
            var model = new TopicMessageModel
            {
                Type = (int)TopicMessageType.RegisterConfirm,
                Data = request
            };
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(model) };
            var result = SendToKafka(topic, message);
            Console.WriteLine(result);
            _logger.LogInformation(result.ToString());
            return Task.FromResult(new DefaultReply
            {
                Message = "Success"
            });
        }

        public override Task<DefaultReply> ChangeStatus(ChangeStatusRequest request, ServerCallContext context)
        {
            var model = new TopicMessageModel
            {
                Type = (int)TopicMessageType.ChangeStatus,
                Data = request
            };
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(model) };
            var result = SendToKafka(topic, message);
            Console.WriteLine(result);
            _logger.LogInformation(result.ToString());
            return Task.FromResult(new DefaultReply
            {
                Message = "Success"
            });
        }

        public override Task<DefaultReply> GetUserList(UserListRequest request, ServerCallContext context)
        {
            var model = new TopicMessageModel
            {
                Type = (int)TopicMessageType.UserList,
                Data = request
            };
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(model) };
            var result = SendToKafka(topic, message);
            Console.WriteLine(result);
            _logger.LogInformation(result.ToString());
            return Task.FromResult(new DefaultReply
            {
                Message = "Success"
            });
        }

        private Object SendToKafka(string topic, Message<Null, string> message)
        {
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    return producer.ProduceAsync(topic, message)
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