using Newtonsoft.Json;
using VkBot.Models;
using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace VkBot.Services
{
    public class BotService
    {
        private readonly VkApi _vkApi;
        private readonly AppDbContext _db;
        private readonly TemplateService _templates;

        public BotService(VkApi vkApi, AppDbContext db, TemplateService templates)
        {
            _vkApi = vkApi;
            _db = db;
            _templates = templates;
        }

        public async Task HandleMessage(Message message)
        {
            if (message.MessageObject.Message.FromId == null) return;

            var userId = message.MessageObject.Message.FromId;
            var userState = await _db.UserStates.FindAsync(userId) ?? new UserState { UserId = userId };

            if (!string.IsNullOrEmpty(message.Payload))
            {
                try
                {
                    dynamic payload = JsonConvert.DeserializeObject(message.Payload);
                    userState.CurrentMenuLevel = payload.command == "back" ? payload.@to : payload.command;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Payload parsing error: {ex.Message}");
                }
            }

            try
            {
                switch (userState.CurrentMenuLevel)
                {
                    case "main_menu":
                        await SendKeyboard(userId, "main_menu");
                        break;
                    case "education_menu":
                        await SendKeyboard(userId, "education_menu");
                        break;
                    case "robotics":
                        await SendCarousel(userId, "robotics");
                        break;
                    default:
                        await SendKeyboard(userId, "main_menu");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }

        private async Task SendKeyboard(long userId, string templateName)
        {
            var template = _templates.GetKeyboard(templateName);
            var keyboard = new KeyboardBuilder();
            if (template.OneTime)
                keyboard.SetOneTime();

            foreach (var row in template.Buttons)
            {
                var rowButtons = row.Select(button => new MessageKeyboardButton
                {
                    Color = KeyboardButtonColor.Default,
                    Action = new MessageKeyboardButtonAction
                    {
                        Type = KeyboardButtonActionType.Text,
                        Label = button.Text,
                        Payload = JsonConvert.SerializeObject(new
                        {
                            command = button.Payload,
                            to = button.Payload
                        })
                    }
                }).ToArray();

                keyboard.AddLine();
            }

            await _vkApi.Messages.SendAsync(new MessagesSendParams
            {
                UserId = userId,
                Message = template.Message,
                Keyboard = keyboard.Build(),
                RandomId = Environment.TickCount
            });
        }

        private async Task SendCarousel(long userId, string templateName)
        {
            var template = _templates.GetCarousel(templateName);
            var carousel = new List<MediaAttachment>();

            foreach (var element in template.Elements)
            {
                if (!string.IsNullOrEmpty(element.PhotoId))
                {
                    var photos = await _vkApi.Photo.GetByIdAsync(new[] { element.PhotoId });
                    var photo = photos.FirstOrDefault();

                    if (photo != null)
                    {
                        carousel.Add(photo);
                    }
                }
            }

            await _vkApi.Messages.SendAsync(new MessagesSendParams
            {
                UserId = userId,
                Message = template.Message,
                Attachments = carousel,
                RandomId = Environment.TickCount
            });
        }
    }
}
