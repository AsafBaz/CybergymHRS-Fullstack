﻿using System.ComponentModel.DataAnnotations;

namespace CybergymHRS.Core.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

    }
}
