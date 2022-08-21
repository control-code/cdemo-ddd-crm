using System;

namespace Cdemo.Entities
{
    public class Entity<TState>
    {
        public Guid Id { get; private set; }
        public TState State { get; private set; }

        public Entity(Guid id, TState state)
        {
            Id = id;
            State = state;
        }
    }
}
