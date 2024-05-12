using Zadanie7.Models.DTOs;

namespace Zadanie7
{
    public interface ITripsRepository
    {
        public Task<IEnumerable<TripDTO>> GetTripsAsync();
    }
}
