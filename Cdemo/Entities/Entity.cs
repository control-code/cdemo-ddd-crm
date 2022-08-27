using System;

namespace Cdemo.Entities
{
    public class Entity<TState>
    {
        private TState _state;

        public Guid Id { get; private set; }
        public TState State { get => _state; protected set { _state = value; StateChanged = true; } }
        public bool StateChanged { get; private set; }

        public Entity(Guid id, TState state)
        {
			_state = state;

			Id = id;
            StateChanged = false;
		}

        public void Saved()
        {
            StateChanged = false;
        }
    }
}
