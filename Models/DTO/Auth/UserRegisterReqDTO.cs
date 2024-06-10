using System.ComponentModel.DataAnnotations;
using TabProjectServer.Models.Domain;

namespace TabProjectServer.Models.DTO.Auth
{
    public class UserRegisterReqDTO
    {
        [MinLength(4), MaxLength(30), Required]
        public string Username { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }

        [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain at least one lowercase letter, one uppercase letter, and one digit.")]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public UserRole? Role { get; set; } = UserRole.User;
        public string? AdminRoleKey { get; set; }
    }
}
