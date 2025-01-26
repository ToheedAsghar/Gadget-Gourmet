
namespace Core.Interface
{
	public interface IGenericRepository<T>
	{
		public Task<IEnumerable<T>> GetAll();
		public Task<T> GetById(int Id);
		public Task Insert(T Entity);
		public Task Update(T Entity);
		public Task Delete(int Id);
	}
}
