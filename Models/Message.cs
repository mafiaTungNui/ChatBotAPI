using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBotAPI.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [Required]
        public int ConversationID { get; set; }

        [Required]
        [StringLength(20)]
        public required string Sender { get; set; } 

        [Required]
        public required string MessageText { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
