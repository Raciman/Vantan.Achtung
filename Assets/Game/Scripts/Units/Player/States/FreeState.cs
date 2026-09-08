
using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class FreeState : PlayerStateBase
    {
        public bool IsSprinting { get; private set; }
        
        public FreeState(PlayerContext ctx) : base(ctx) { }
    
        public override void Enter()
        {
        
        }

        public override void Tick(float deltaTime)
        {
            float speedMultiplier = 1.0f; 

            IsSprinting = Ctx.Intent.WantsSprint
                          && Ctx.Intent.MoveDirection.sqrMagnitude > 0.001f
                          && !Ctx.Stance.IsAiming
                          && !Ctx.Stance.IsReloading;
            
            if (IsSprinting)
                speedMultiplier = Ctx.Config.SprintSpeedMultiplier;
            else if(Ctx.Stance.IsReloading)
                speedMultiplier = Ctx.Config.ReloadSpeedMultiplier;
            
            Ctx.Motor.SetMoveIntent(Ctx.Intent.MoveDirection);
            Ctx.Motor.SetSpeedMultiplier(speedMultiplier);
            
            if (Ctx.Intent.WantsAim)
                Ctx.Look.SetTarget(Ctx.Intent.LookDirection);
            else if(Ctx.Intent.MoveDirection.sqrMagnitude > 0.001f)
                Ctx.Look.SetTarget(Ctx.Intent.MoveDirection);
            
        }

        public override void Exit()
        {
        
        }
    }
}

