using System.ComponentModel.DataAnnotations;

namespace OnlyMembers.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(10)]
        public string Mobile { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Otp { get; set; } 
        public bool IsVerified { get; set; } = false;

        public int Points { get; set; } = 0;
    }
}
