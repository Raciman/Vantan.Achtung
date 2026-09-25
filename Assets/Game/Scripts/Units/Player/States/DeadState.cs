using UnityEngine;

namespace Ach.Units.Player
{
    public class DeadState : PlayerStateBase
    {
        public DeadState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            Ctx.Motor.SetMoveIntent(Vector3.zero);
            Ctx.Animator.SwitchAnimator(false);
            Ctx.Ragdoll.SetRagdoll(true);
            Ctx.Weapon.DropWeapon();
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}