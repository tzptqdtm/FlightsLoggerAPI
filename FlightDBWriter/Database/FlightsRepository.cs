namespace FlightDBWriter.Database;

public class FlightsRepository : IFlightsRepository
{
    private readonly DataContext _db;

    public FlightsRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<bool> Add(Flight entity)
    {
        await _db.Flights.AddAsync(entity);
        return await Save();
    }

    public async Task<bool> Save()
    {
        var changes = await _db.SaveChangesAsync();
        return changes > 0;
    }
}