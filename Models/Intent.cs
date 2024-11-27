using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatBotAPI.Models
{
    public class Intent
    {
        [Key]
        public int IntentID { get; set; } 

        public required string IntentName { get; set; }

        public virtual required ICollection<Response> Responses { get; set; }
    }
}
