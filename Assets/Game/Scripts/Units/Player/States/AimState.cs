
namespace Ach.Units.Player
{
    public sealed class AimState : PlayerStateBase
    {
        public AimState(PlayerContext ctx) : base(ctx)
        {
            
        }

        public override void Enter()
        {
            Ctx.Animator.PlayStanceAim();
        }

        public override void Tick(float deltaTime)
        {
            Ctx.Look.SetTarget(Ctx.Intent.LookDirection);
        }

        public override void Exit()
        {
            
        }
    }
}