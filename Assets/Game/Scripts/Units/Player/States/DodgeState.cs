using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class DodgeState : PlayerStateBase
    {
        private float _impulseVelocity = 15f; //TODO stats
        
        public DodgeState(PlayerContext ctx) : base(ctx)
        {
            
        }

        public override void Enter()
        {
            
        }

        public override void Tick(float deltaTime)
        {
            //Ctx.Motor.SetImpulse(Vector3.forward);
        }

        public override void Exit()
        {
            
        }
    }
}