using System.ComponentModel.DataAnnotations;

namespace CybergymHRS.Core.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string RoomNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        public int Capacity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PricePerNight { get; set; }

        public bool IsAvailable { get; set; } = true;

    }
}
