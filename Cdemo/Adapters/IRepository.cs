using Cdemo.Entities;

namespace Cdemo.Adapters
{
	public interface IRepository<T, TState> where T : Entity<TState>
	{
		Task<T?> Get(Guid id);

		Task Add(T entity);

		Task Update(T entity);
	}
}
