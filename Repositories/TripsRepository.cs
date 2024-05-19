using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Zadanie7.Models;
using Zadanie7.Models.DTOs;
using Zadanie7.Repositories;

namespace Zadanie7.Properties
{
    public class TripsRepository : ITripsRepository
    {
        private readonly S26294Context _context;
        public TripsRepository(S26294Context context) {  _context = context; }
        public async Task<IEnumerable<TripDTO>> GetTripsAsync()
        {
            var result = await _context
                .Trips
                .OrderByDescending(e => e.DateFrom)
                .Select(e => new TripDTO { 
                Name = e.Name,
                Description = e.Description,
                DateFrom = e.DateFrom,   //DateFrom = DateOnly.FromDateTime(e.DateFrom)
                DateTo = e.DateTo,       //DateTo = DateOnly.FromDateTime(e.DateTo)
                MaxPeople = e.MaxPeople,
                Countries = e.IdCountries
                .Select(e =>
                new CountryDTO {Name = e.Name}),
                Clients = e.ClientTrips
                .Select(e => new ClientDTO { FirstName = e.IdClientNavigation.FirstName, LastName = e.IdClientNavigation.LastName})}).ToListAsync();

            return result;
        }

        public async Task<int> DeleteClientAsync(int idClient) {

            var client = await _context
                .Clients
                .Include(e => e.ClientTrips)
                .FirstOrDefaultAsync(e => e.IdClient == idClient);

            if (client == null)
            {
                return 0;
            }

            if (client.ClientTrips.Any())
            {
                return -1;
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return 1;

        }

        public async Task<int> AddClientToTripAsync(int idTrip, ClientTripDTO client) {

            bool clientExists = await _context.Clients.AnyAsync(e => e.Pesel == client.Pesel);
            if (!clientExists)
            {
                _context.Clients.Add(new Client
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.Email,
                    Telephone = client.Telephone,
                    Pesel = client.Pesel,
                });
                await _context.SaveChangesAsync();
            }

            bool tripExists = await _context.Trips.AnyAsync(e => e.IdTrip == idTrip);
            if (!tripExists) 
            {
                return -1;
            }

            bool isClientBooked = await _context.ClientTrips.AnyAsync(
                e => e.IdTrip == idTrip && e.IdClientNavigation.Pesel == client.Pesel);
            if(isClientBooked)
            {
                return 0;
            }

            var idClient = await _context.Clients.FirstOrDefaultAsync(e => e.Pesel == client.Pesel);
            _context.ClientTrips.Add(new ClientTrip
            {
                IdClient = idClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.UtcNow,
                PaymentDate = client.PaymentDate != null ? client.PaymentDate : null,
            });
            await _context.SaveChangesAsync();

            return 1;
        }
    }
}
