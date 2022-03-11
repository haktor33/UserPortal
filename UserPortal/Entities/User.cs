using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserPortal.Entities
{
    [Table("users")]
    public class User 
    {
        [Key, Column("id")]
        public long Id { get; set; }
        [Column("first_name"), MaxLength(100)]
        public string FirstName { get; set; }
        [Column("last_name"), MaxLength(100)]
        public string LastName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("username"), MaxLength(50)]
        public string? Username { get; set; }
        [Column("password"), MaxLength(100), JsonIgnore]
        public string Password { get; set; }
        [Column("status")]
        public bool Active { get; set; } = false;
        [Column("registered_date")]
        public DateTime? CreateDate { get; set; }
    }
}