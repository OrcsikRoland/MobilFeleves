using System.Collections.Generic;
using System.Threading.Tasks;
using MobilFeleves.Models;

namespace MobilFeleves.Services;

public interface ITripRepository
{
    Task InitializeAsync();
    Task<IReadOnlyList<Trip>> GetTripsAsync();
    Task<Trip?> GetTripAsync(int id);
    Task<int> SaveTripAsync(Trip trip);
    Task<int> DeleteTripAsync(Trip trip);
}
