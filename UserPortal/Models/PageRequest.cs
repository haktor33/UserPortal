using System.ComponentModel.DataAnnotations;

namespace UserPortal.Models
{
    public class PageRequest
    {
        public int Current { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}