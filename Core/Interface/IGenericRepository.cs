
namespace Core.Interface
{
	public interface IGenericRepository<T>
	{
		Task<IEnumerable<T>> GetAll();
		Task<T> GetById(int Id);
		Task Insert(T Entity);
		Task Update(T Entity);
		Task Delete(int Id);
	}
}
