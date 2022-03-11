using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using UserPortal.Extensions;
using UserPortal.Services;

namespace UserPortal.Controllers
{
    [ApiController]
    public class KafkaProducerController : BaseController
    {
        private readonly KafkaService _kafkaService;
        public KafkaProducerController(KafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }
        [HttpPost]
        public IActionResult Post([FromQuery] string message)
        {
            return Created(string.Empty, _kafkaService.SendToKafka(message));
        }


    }
}
