using System;
using SQLite;

namespace MobilFeleves.Models;

public class Trip
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public double DistanceKm { get; set; }

    public int ElevationGainM { get; set; }

    public DateTime Date { get; set; } = DateTime.Today;

    public int DurationMinutes { get; set; }

    public double StartLatitude { get; set; }

    public double StartLongitude { get; set; }

    public string? PhotoPath { get; set; }

    public string? WeatherSummary { get; set; }

    public string? SyncStatus { get; set; }

    public string DurationText => TimeSpan.FromMinutes(DurationMinutes).ToString(@"hh\:mm");
}
