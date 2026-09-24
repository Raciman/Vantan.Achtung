using Ach.FSM;
using UnityEngine;

namespace Ach.Units.Enemies
{
    public abstract class EnemyStateBase : IState
    {
        protected readonly EnemyContext Ctx;

        protected EnemyStateBase(EnemyContext ctx)
        {
            Ctx = ctx;
        }
        public abstract void Enter();

        public abstract void Tick(float deltaTime);

        public abstract void Exit();
    }
}

