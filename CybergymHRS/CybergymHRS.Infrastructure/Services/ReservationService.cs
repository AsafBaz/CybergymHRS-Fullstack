using CybergymHRS.Core.Interfaces;
using CybergymHRS.Core.Models;
using CybergymHRS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CybergymHRS.Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly CybergymHrsContext _context;

        public ReservationService(CybergymHrsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<Reservation> CreateReservationAsync(Reservation Reservation)
        {
            await ValidateRoomAvailability(Reservation);

            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            return Reservation;
        }

        public async Task<Reservation> UpdateReservationAsync(Reservation Reservation)
        {
            await ValidateRoomAvailability(Reservation, Reservation.Id); 

            _context.Reservations.Update(Reservation);
            await _context.SaveChangesAsync();

            return Reservation;
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            var Reservation = await _context.Reservations.FindAsync(id);
            if (Reservation == null) return false;

            _context.Reservations.Remove(Reservation);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task ValidateRoomAvailability(Reservation reservation, int? existingReservationId = null)
        {
            var query = _context.Reservations.Where(r =>
                r.RoomId == reservation.RoomId &&
                r.CheckInDate< reservation.CheckOutDate &&
                r.CheckOutDate > reservation.CheckInDate);

            if (existingReservationId.HasValue)
            {
                // Exclude the reservation itself when updating
                query = query.Where(r => r.Id != existingReservationId.Value);
            }

            bool hasOverlap = await query.AnyAsync();

            if (hasOverlap)
            {
                throw new InvalidOperationException("The room is already booked for the selected dates and times.");
            }
        }
    }
}
