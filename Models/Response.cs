using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBotAPI.Models
{
    public class Response
    {
        [Key]
        public int ResponseID { get; set; }

        public int IntentID { get; set; }

        [ForeignKey("IntentID")]
        public virtual required Intent IntentNavigation { get; set; } 

        public required string ResponseText { get; set; } 
    }
}
