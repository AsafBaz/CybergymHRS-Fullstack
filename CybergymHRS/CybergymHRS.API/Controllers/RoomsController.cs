using CybergymHRS.Core.Interfaces;
using CybergymHRS.Core.Models;
using CybergymHRS.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CybergymHRS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomsController> _logger;

        public RoomsController(IRoomService roomService, ILogger<RoomsController> logger)
        {
            _roomService = roomService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all rooms...");
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching room ID {RoomId}", id);
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                _logger.LogWarning("Room ID {RoomId} not found.", id);
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto roomDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid room create request.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new room...");

            var room = new Room
            {
                RoomNumber = roomDto.RoomNumber,
                Type = roomDto.Type,
                Capacity = roomDto.Capacity,
                PricePerNight = roomDto.PricePerNight,
                IsAvailable = roomDto.IsAvailable
            };

            try
            {
                var createdRoom = await _roomService.CreateRoomAsync(room);
                return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Rooms_RoomNumber") == true)
            {
                _logger.LogWarning("Attempted to create room with duplicate RoomNumber {RoomNumber}", roomDto.RoomNumber);
                return BadRequest(new { error = $"Room number {roomDto.RoomNumber} already exists." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating room.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Room room)
        {
            if (id != room.Id)
            {
                _logger.LogWarning("Room ID mismatch for update: {ProvidedId} != {RoomId}", id, room.Id);
                return BadRequest("Room ID mismatch.");
            }

            _logger.LogInformation("Updating room ID {RoomId}", id);
            try
            { 
                var updatedRoom = await _roomService.UpdateRoomAsync(room);
                return Ok(updatedRoom);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Rooms_RoomNumber") == true)
            {
                _logger.LogWarning("Attempted to update room with duplicate RoomNumber {RoomNumber}", room.RoomNumber);
                return BadRequest(new { error = $"Room number {room.RoomNumber} already exists." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating room.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting room ID {RoomId}", id);

            var deleted = await _roomService.DeleteRoomAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Room ID {RoomId} not found for deletion.", id);
                return NotFound();
            }
            return NoContent();
        }
    }
}
