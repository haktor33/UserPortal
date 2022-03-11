using System.ComponentModel.DataAnnotations;

namespace UserPortal.Models
{
    public class ChangeStatusRequest
    {
        [Required]
        public long UserId { get; set; }

        public bool Active { get; set; } = false;
    }
}