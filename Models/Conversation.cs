using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBotAPI.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationID { get; set; }

        [Required]
        public int UserID { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; } 

        [StringLength(20)]
        public required string Status { get; set; }
    }
}
