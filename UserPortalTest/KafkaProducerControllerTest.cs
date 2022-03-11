
using UserPortal.Services;

using Xunit;

namespace UserPortal.Controllers.Tests
{
    public class KafkaProducerControllerTest
    {
        [Fact]
        public void PostTest()
        {
            var result = KafkaService.SendToKafka("testMessage"); ;
            Assert.Equal("testMessage", result.Value);
        }
    }
}