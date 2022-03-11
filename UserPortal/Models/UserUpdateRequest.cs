using System.ComponentModel.DataAnnotations;
using UserPortal.Entities;

namespace UserPortal.Models
{
    public class UserUpdateRequest
    {
        [Required]
        public long Id { get; set; }
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
        public User ReverseMapToUser(User user)
        {
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Username = Username;
            user.Password = Password;
            user.Email = Email;

            return user;
        }
    }
}