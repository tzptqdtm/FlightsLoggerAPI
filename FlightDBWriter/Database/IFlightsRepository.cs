namespace FlightDBWriter.Database;

public interface IFlightsRepository
{
    Task<bool> Add(Flight entity);
    Task<bool> Save();
}