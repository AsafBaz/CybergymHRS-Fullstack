using CybergymHRS.Core.Models;

namespace CybergymHRS.Core.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<List<Reservation>> GetReservationsByUserIdAsync(Guid userId);

        Task<Reservation> CreateReservationAsync(Reservation Reservation);
        Task<Reservation> UpdateReservationAsync(Reservation Reservation);
        Task<bool> DeleteReservationAsync(int id);
    }
}
