using System;
using System.ComponentModel.DataAnnotations;

namespace ChatBotAPI.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(50)]
        public required string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
