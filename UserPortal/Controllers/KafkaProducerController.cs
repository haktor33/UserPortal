using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using UserPortal.Extensions;
using UserPortal.Services;

namespace UserPortal.Controllers
{
    [ApiController]
    public class KafkaProducerController : BaseController
    {
        [HttpPost]
        public IActionResult Post([FromQuery] string message)
        {
            return Created(string.Empty, KafkaService.SendToKafka(message));
        }


    }
}
