using System.ComponentModel.DataAnnotations;

namespace VkBot.Models
{
    public class UserState
    {
        [Key]
        public long UserId { get; set; }          // ID пользователя VK
        public string CurrentMenuLevel { get; set; } = "main_menu";
        public string? PreviousMenuLevel { get; set; }  // Для кнопки "Назад"
        public DateTime LastActivity { get; set; } = DateTime.UtcNow;
    }

}
