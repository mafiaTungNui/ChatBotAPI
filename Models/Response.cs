using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBotAPI.Models
{
    public class Response
    {
        [Key]
        public int ResponseID { get; set; }

        public int IntentID { get; set; } // Sử dụng int thay vì string

        [ForeignKey("IntentID")]
        public virtual required Intent IntentNavigation { get; set; } // Khóa ngoại đến Intent

        public required string ResponseText { get; set; } // Thêm thuộc tính ResponseText
    }
}
