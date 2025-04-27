using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CybergymHRS.Core.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Booked";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Room Room { get; set; }
    }
}
