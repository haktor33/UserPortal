using System.ComponentModel.DataAnnotations;
using UserPortal.Entities;

namespace UserPortal.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public User ReverseMapToUser()
        {
            var user = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Username = Username,
                Password = Password,
                Email = Email
            };

            return user;
        }
    }
}