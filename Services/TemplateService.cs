using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace VkBot.Services
{
    public class TemplateService
    {
        private readonly IMemoryCache _cache;
        private readonly string _templatesPath = "Keyboards";

        public TemplateService(IMemoryCache cache) => _cache = cache;

        public KeyboardTemplate GetKeyboard(string name)
        {
            return _cache.GetOrCreate($"keyboard_{name}", entry =>
            {
                var json = File.ReadAllText($"{_templatesPath}/{name}.json");
                return JsonSerializer.Deserialize<KeyboardTemplate>(json);
            });
        }

        public CarouselTemplate GetCarousel(string name)
        {
            return _cache.GetOrCreate($"carousel_{name}", entry =>
            {
                var json = File.ReadAllText($"{_templatesPath}/carousels/{name}.json");
                return JsonSerializer.Deserialize<CarouselTemplate>(json);
            });
        }
    }

    // Модели для десериализации JSON
    public class KeyboardTemplate
    {
        public string Message { get; set; }
        public List<List<ButtonTemplate>> Buttons { get; set; }
        public bool OneTime { get; set; }
    }

    public class ButtonTemplate
    {
        public string Text { get; set; }
        public Dictionary<string, object> Payload { get; set; }
    }

    public class CarouselTemplate
    {
        public string Message { get; set; }
        public List<CarouselElementTemplate> Elements { get; set; }
    }

    public class CarouselElementTemplate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoId { get; set; }
        public List<ButtonTemplate> Buttons { get; set; }
    }

}
