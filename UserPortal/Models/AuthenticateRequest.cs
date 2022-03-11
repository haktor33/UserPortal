using System.ComponentModel.DataAnnotations;

namespace UserPortal.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "En az sekiz karakterli bir kullanýcý adý giriniz.")]
        public string Username { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "En az sekiz karakterli bir þifre giriniz.")]
        public string Password { get; set; }
    }
}