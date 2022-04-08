using Microsoft.EntityFrameworkCore;

namespace FlightDBWriter.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Flight> Flights { get; set; }
}