
using UserPortal.Services;

using Xunit;

namespace UserPortal.Controllers.Tests
{
    public class KafkaProducerControllerTest
    {
        [Fact]
        public void PostTest()
        {
            var result = KafkaService.SendToKafka("testMessage") as Confluent.Kafka.DeliveryResult<Confluent.Kafka.Null, string>;
            Assert.Equal("testMessage", result.Value);
        }
    }
}