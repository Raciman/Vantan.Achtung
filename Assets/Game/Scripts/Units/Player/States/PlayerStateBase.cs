using Ach.FSM;

namespace Units.Player
{
    public abstract class PlayerStateBase : IState
    {
        protected readonly PlayerContext Ctx;

        protected PlayerStateBase(PlayerContext ctx)
        {
            Ctx = ctx;
        }
        public abstract void Enter();

        public abstract void Tick(float deltaTime);

        public abstract void Exit();
    }
}

