using Cdemo.Adapters;
using Cdemo.Entities;
using System.Collections.Concurrent;
using System.Reflection;

namespace Cdemo.AdaptersImpl
{
	public class InMemoryRepository<T, TState> : IRepository<T, TState> where T : Entity<TState> where TState : class
	{
		private readonly ConcurrentDictionary<Guid, TState> _states = new ConcurrentDictionary<Guid, TState>();

		public IDictionary<Guid, TState> States => _states;

		public TState GetState(Guid id)
		{
			return _states[id];
		}

		public Task<T?> Get(Guid id)
		{
			var state = _states.ContainsKey(id) ? _states[id] : null;
			if (state == null) return Task.FromResult((T?)null);

			ConstructorInfo? ctor = typeof(T).GetConstructor(new[] { typeof(Guid), typeof(TState) });
			object? instance = ctor?.Invoke(new object[] { id, state });

			return Task.FromResult((T?)instance);
		}

		public Task Add(T entity)
		{
			_states[entity.Id] = entity.State;
			return Task.CompletedTask;
		}

		public Task Update(T entity)
		{
			if (entity.StateChanged)
			{
				_states[entity.Id] = entity.State;
				entity.Saved();
			}
			return Task.CompletedTask;
		}
	}
}
