using Zadanie7.Models;
using Zadanie7.Models.DTOs;

namespace Zadanie7.Properties
{
    public class TripsRepository : ITripsRepository
    {
        private readonly MasterContext _context;
        public TripsRepository(MasterContext context) {  _context = context; }
        public Task<IEnumerable<TripDTO>> GetTripsAsync()
        {
            var result = _context
                .Trips
                .Select(e => new TripDTO { 
                Name = e.Name,
                Description = e.Description,
                DateFrom = DateOnly.FromDateTime(e.DateFrom),
                DateTo = DateOnly.FromDateTime(e.DateTo),
                MaxPeople = e.MaxPeople,
                Countries = e.IdCountries.
                Select(e=>
                new CountryDTO { Name = e.Name}),
                Clients = e.ClientTrips
                .Select(e=> new ClientDTO { FristName = e.IdClientNavigation.FirstName, LastName = e.IdClientNavigation.LastName})}).ToList();

            return result;
        }
    }
}
