using Ach.FSM;
using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class IdleState : PlayerStateBase
    {
        public IdleState(PlayerContext ctx) : base(ctx)
        {
            
        }

        public override void Enter()
        {
            Ctx.Animator.PlayStanceIdle();
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}

