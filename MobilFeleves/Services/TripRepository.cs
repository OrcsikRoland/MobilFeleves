using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using MobilFeleves.Models;
using SQLite;

namespace MobilFeleves.Services;

public class TripRepository : ITripRepository
{
    private const string DatabaseFileName = "trips.db3";
    private SQLiteAsyncConnection? _database;
    private bool _initialized;

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
        _database = new SQLiteAsyncConnection(databasePath);
        await _database.CreateTableAsync<Trip>();
        _initialized = true;
    }

    private SQLiteAsyncConnection Database => _database ?? throw new InvalidOperationException("Database not initialized");

    public async Task<IReadOnlyList<Trip>> GetTripsAsync()
    {
        await InitializeAsync();
        return await Database.Table<Trip>().OrderByDescending(t => t.Date).ToListAsync();
    }

    public async Task<Trip?> GetTripAsync(int id)
    {
        await InitializeAsync();
        return await Database.Table<Trip>().Where(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveTripAsync(Trip trip)
    {
        await InitializeAsync();
        if (string.IsNullOrWhiteSpace(trip.Title))
        {
            throw new ArgumentException("A túra neve kötelező.", nameof(trip));
        }

        return trip.Id == 0
            ? await Database.InsertAsync(trip)
            : await Database.UpdateAsync(trip);
    }

    public async Task<int> DeleteTripAsync(Trip trip)
    {
        await InitializeAsync();
        if (trip.Id == 0)
        {
            return 0;
        }

        return await Database.DeleteAsync(trip);
    }
}
