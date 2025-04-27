// ReservationsController.cs
using CybergymHRS.Core.DTOs;
using CybergymHRS.Core.Interfaces;
using CybergymHRS.Core.Models;
using CybergymHRS.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CybergymHRS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _ReservationService;
        private readonly ILogger<ReservationsController> _logger;

        public ReservationsController(IReservationService reservationService, ILogger<ReservationsController> logger)
        {
            _ReservationService = reservationService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching reservations for current user...");

            try
            {
                var userId = User?.FindFirst("sub")?.Value ?? User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in token.");
                    return Unauthorized("User ID missing from token.");
                }

                var reservations = await _ReservationService.GetReservationsByUserIdAsync(Guid.Parse(userId));
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservations.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching reservation ID {ReservationId}", id);

            try
            {
                var reservation = await _ReservationService.GetReservationByIdAsync(id);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation ID {ReservationId} not found", id);
                    return NotFound();
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching reservation ID {ReservationId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto reservationDto)
        {
            if (reservationDto.CheckInDate >= reservationDto.CheckOutDate)
            {
                _logger.LogWarning("Invalid reservation dates: CheckIn {CheckIn} - CheckOut {CheckOut}", reservationDto.CheckInDate, reservationDto.CheckOutDate);
                return BadRequest("Check-in date must be before check-out date.");
            }

            _logger.LogInformation("Creating new reservation...");

            var userId = User?.FindFirst("sub")?.Value ?? User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID not found in token.");
                return Unauthorized("User ID missing from token.");
            }

            var reservation = new Reservation
            {
                RoomId = reservationDto.RoomId,
                CheckInDate = reservationDto.CheckInDate,
                CheckOutDate = reservationDto.CheckOutDate,
                UserId = Guid.Parse(userId),
                Status = "Booked",
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                var createdReservation = await _ReservationService.CreateReservationAsync(reservation);
                return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation failed during reservation update.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a reservation.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateReservationDto reservationDto)
        {
            if (id != reservationDto.Id)
                return BadRequest("Reservation ID mismatch.");

            if (reservationDto.CheckInDate >= reservationDto.CheckOutDate)
            {
                _logger.LogWarning("Invalid reservation dates: CheckIn {CheckIn} - CheckOut {CheckOut}", reservationDto.CheckInDate, reservationDto.CheckOutDate);
                return BadRequest("Check-in date must be before check-out date.");
            }
            _logger.LogInformation("Updating reservation ID {ReservationId}", id);

            try
            {
                var existingReservation = await _ReservationService.GetReservationByIdAsync(id);

                if (existingReservation == null)
                    return NotFound();

                existingReservation.CheckInDate = reservationDto.CheckInDate;
                existingReservation.CheckOutDate = reservationDto.CheckOutDate;

                await _ReservationService.UpdateReservationAsync(existingReservation);

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Validation failed during reservation update.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating reservation ID {ReservationId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting reservation ID {ReservationId}", id);

            try
            {
                var reservation = await _ReservationService.GetReservationByIdAsync(id);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation ID {ReservationId} not found for deletion", id);
                    return NotFound();
                }

                await _ReservationService.DeleteReservationAsync(reservation.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting reservation ID {ReservationId}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
