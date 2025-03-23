using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Role { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; } 
    }
}
