namespace HospitalManagementSystem;

// generic example - generic interface with type parameter T
public interface IRepository<T>
{
    T? GetById(int id);
    List<T> GetAll();
    void Add(T entity);
    void Remove(T entity);
}