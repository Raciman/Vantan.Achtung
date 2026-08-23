using System;

namespace Ach.FSM
{
    public interface IState
    {
        void Enter();
        void Tick(float deltaTime);
        void Exit();
    }

    public abstract class StateBase : IState
    {
        public virtual void Enter() { }

        public virtual void Tick(float deltaTime) { }

        public virtual void Exit() { }
    }

    public readonly struct Transition
    {
        public readonly IState To;
        public readonly Func<bool> Condition;

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}

