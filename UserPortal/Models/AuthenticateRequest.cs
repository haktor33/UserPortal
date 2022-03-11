using System.ComponentModel.DataAnnotations;

namespace UserPortal.Models
{
    public class AuthenticateRequest
    {
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "En az sekiz karakterli bir kullan�c� ad� giriniz.")]
        public string Username { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 1, ErrorMessage = "En az sekiz karakterli bir �ifre giriniz.")]
        public string Password { get; set; }
    }
}