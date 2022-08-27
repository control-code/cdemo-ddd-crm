using Cdemo.Adapters;
using Cdemo.Entities;

namespace Cdemo.AdaptersImpl
{
	public class InMemoryRepository<T, TState> : IRepository<T, TState> where T : Entity<TState>
	{
		private readonly SortedList<Guid, T> _entities = new SortedList<Guid, T>();

		public ICollection<T> Entities => _entities.Values;
		public Task<T?> Get(Guid id)
		{
			if (_entities.ContainsKey(id))
			{
				return Task.FromResult((T?)_entities[id]);
			}

			return Task.FromResult((T?)null);
		}

		public Task Add(T entity)
		{
			_entities.Add(entity.Id, entity);
			return Task.CompletedTask;
		}

		public Task Update(T entity)
		{
			_entities[entity.Id] = entity;
			return Task.CompletedTask;
		}
	}
}
