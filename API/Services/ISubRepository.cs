namespace API.Services;

public interface ISubRepository<T1>
{
    Task<IEnumerable<T1>> Get(int id);
    Task<T1?> Add(int id, int entityID);
    Task<T1?> Delete(int id, int entityID);
}