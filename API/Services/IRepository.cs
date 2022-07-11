namespace API.Services;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> Get(int id);
    Task<T?> Add(T entity);
    Task<T?> Update(T entity);
    Task<T?> Delete(int id);
}