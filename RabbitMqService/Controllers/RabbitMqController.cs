using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMqService.Interfaces;

namespace RabbitMqService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IRabbitMQPublisher _publisher;
        private readonly ILogger<RabbitMqController> _logger;

        public RabbitMqController(IRabbitMQPublisher publisher, ILogger<RabbitMqController> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string message)
        {
            _logger.LogInformation("Received message: {Message}", message);
            await _publisher.PublishAsync(message);
            return Ok();
        }
    }
}
