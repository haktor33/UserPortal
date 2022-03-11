
using Microsoft.Extensions.DependencyInjection;
using UserPortal.Enums;
using UserPortal.Models;
using UserPortal.Services;
using UserPortalTest;
using Xunit;

namespace UserPortal.Controllers.Tests
{
    public class KafkaServiceTest
    {
        private readonly KafkaService _kafkaService;

        public KafkaServiceTest()
        {
            _kafkaService = new KafkaService(null);
        }
        [Fact]
        public async void GetUserListTest()
        {
            var list = await _kafkaService.GetUserList(new PageRequest { Current = 1, PageSize = 1 });
            Assert.NotNull(list);
        }

        [Fact]
        public async void ResponseForTopicMesseageTypeTest()
        {
            var model = new TopicMessageModel
            {
                Type = (int)TopicMessageType.Test,
                Data = "Mock Data",
            };
            var result = await _kafkaService.ResponseForTopicMesseageType(model);
            Assert.Equal("{\"Type\":4,\"Data\":\"Mock Data\"}", result.Value);
        }

        [Fact]
        public void SendToKafkaTest()
        {
            var result = KafkaService.SendToKafka("Sending Kafka Test Message");
            Assert.Equal("Sending Kafka Test Message", result.Value);
        }
    }
}