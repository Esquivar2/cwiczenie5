using Zadanie7.Models.DTOs;

namespace Zadanie7.Repositories
{
    public interface ITripsRepository
    {
        public Task<IEnumerable<TripDTO>> GetTripsAsync();
        public Task<int> DeleteClientAsync(int idClient);
        public Task<int> AddClientToTripAsync(int idClient, ClientTripDTO client);
    }
}
