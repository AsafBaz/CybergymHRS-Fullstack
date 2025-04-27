using System.ComponentModel.DataAnnotations;

namespace CybergymHRS.Core.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        public Guid? UserId { get; set; }

        [MaxLength(100)]
        public string Action { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
