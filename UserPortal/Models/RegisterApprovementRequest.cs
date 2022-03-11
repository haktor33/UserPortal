using System.ComponentModel.DataAnnotations;

namespace UserPortal.Models
{
    public class RegisterApprovementRequest
    {
        [Required]
        public long UserId { get; set; }

        public bool Approvement { get; set; } = false;
    }
}