using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using VkBot.Models;
using VkBot.Services;
using VkNet.Model;
using VkBot.Models;

namespace VkBot.Controllers
{
    [ApiController]
    [Route("api/v5.131")]
    public class BotController : ControllerBase
    {
        private readonly BotService _botService;
        private readonly IConfiguration _configuration;

        public BotController(BotService botService, IConfiguration configuration)
        {
            _botService = botService;
            _configuration = configuration;
        }

        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromBody] JsonElement jsonData)
        {
            try
            {
                // Проверяем наличие обязательного поля type
                if (!jsonData.TryGetProperty("type", out var typeProperty))
                {
                    return BadRequest("Invalid callback data format: missing 'type' field");
                }

                string eventType = typeProperty.GetString();

                switch (eventType)
                {
                    case "confirmation":
                        // Возвращаем строку подтверждения из конфигурации
                        var confirmationString = _configuration["VkSettings:ConfirmationString"];
                        if (string.IsNullOrEmpty(confirmationString))
                        {
                            return StatusCode(500, "Confirmation string not configured");
                        }
                        return Ok(confirmationString);

                    case "message_new":
                        if (!jsonData.TryGetProperty("object", out var objectProperty))
                        {
                            return BadRequest("Missing 'object' field for message_new event");
                        }

                        var messageJson = objectProperty.GetProperty("message");
                        var message = JsonSerializer.Deserialize<VkBotIncomingEvent>(messageJson.GetRawText(), new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (message != null)
                        {
                            await _botService.HandleMessage(message);
                        }
                        return Ok();

                    default:
                        // Для других типов событий просто возвращаем ok
                        return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing callback: {ex.Message}");
            }
        }
    }

}
