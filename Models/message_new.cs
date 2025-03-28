using Newtonsoft.Json;

namespace VkBot.Models
{
    /// <summary>
    /// Класс для сериализации входящего json объекта от VK с типом message_new 
    /// V = 5.131
    /// </summary>
    public partial class VkBotIncomingEvent
    {
        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("v")]
        public string V { get; set; }

        [JsonProperty("object")]
        public MessageObject MessageObject { get; set; }
    }

    public partial class MessageObject
    {
        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("client_info")]
        public ClientInfo ClientInfo { get; set; }
    }

    public partial class ClientInfo
    {
        [JsonProperty("button_actions")]
        public List<string> ButtonActions { get; set; }

        [JsonProperty("keyboard")]
        public bool Keyboard { get; set; }

        [JsonProperty("inline_keyboard")]
        public bool InlineKeyboard { get; set; }

        [JsonProperty("carousel")]
        public bool Carousel { get; set; }

        [JsonProperty("lang_id")]
        public long LangId { get; set; }
    }

    public partial class Message
    {
        [JsonProperty("date")]
        public long Date { get; set; }

        [JsonProperty("from_id")]
        public long FromId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("out")]
        public long Out { get; set; }

        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; }

        [JsonProperty("conversation_message_id")]
        public long ConversationMessageId { get; set; }

        [JsonProperty("fwd_messages")]
        public List<object> FwdMessages { get; set; }

        [JsonProperty("important")]
        public bool Important { get; set; }

        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("peer_id")]
        public long PeerId { get; set; }

        [JsonProperty("random_id")]
        public long RandomId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

