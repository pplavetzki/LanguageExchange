using System.ComponentModel.DataAnnotations;

namespace LanguageExchange.Models
{
    public class UserDetail
    {
        [Required]
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(200)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

    }
}
