using UnityEngine;

namespace Ach.Units.Enemies
{
    public sealed class DeadState : EnemyStateBase
    {
        public DeadState(EnemyContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            Ctx.Agent.enabled = false;
            Ctx.Animator.SwitchAnimator(false);
            Ctx.Ragdoll.SetRagdoll(true);
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}

