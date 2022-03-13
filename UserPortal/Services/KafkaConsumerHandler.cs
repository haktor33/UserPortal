using Confluent.Kafka;
using System.Text.Json;
using UserPortal.Helper;
using UserPortal.Interfaces;
using UserPortal.Models;

namespace UserPortal.Services
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly string topic = "UserPortalTopic";
        private readonly IKafkaService _kafkaService;
        private Thread? thread1;

        public KafkaConsumerHandler(IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            thread1 = new Thread(Listener);
            thread1.Start();
            return Task.CompletedTask;
        }

        private void Listener()
        {
          
            var conf = new ConsumerConfig
            {
                GroupId = "st_consumer_group",
                BootstrapServers = Utils.GetKafkaConfigValue(),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using (var builder = new ConsumerBuilder<Ignore,
                string>(conf).Build())
            {
                builder.Subscribe(topic);
                var cancelToken = new CancellationTokenSource();
                try
                {
                    while (true)
                    {                       
                        var consumer = builder.Consume(cancelToken.Token);
                        Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                        var model = JsonSerializer.Deserialize<TopicMessageModel>(consumer.Message.Value);
                        _kafkaService.ResponseForTopicMesseageType(model);
                    }
                }
                catch (Exception)
                {
                    builder.Close();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (thread1 != null && thread1.IsAlive)
            {
                thread1.Abort();
            }
            return Task.CompletedTask;
        }
    }
}
