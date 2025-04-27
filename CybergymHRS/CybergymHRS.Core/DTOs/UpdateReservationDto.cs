using System.ComponentModel.DataAnnotations;

namespace CybergymHRS.Core.DTOs
{
    public class UpdateReservationDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }
    }
}
