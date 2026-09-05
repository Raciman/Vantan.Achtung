using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class FiringState : PlayerStateBase
    {
        private float _elapsed;
        public bool IsRecoveryDone { get; private set; }
        public FiringState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            _elapsed = 0f;
            IsRecoveryDone = false;
            Ctx.Intent.ConsumeFire();
            Ctx.Weapon.Fire();
            Ctx.Animator.PlayFire();
        }

        public override void Tick(float deltaTime)
        {
            _elapsed += deltaTime;
            if(_elapsed >= Ctx.Config.FireRecovery)
                IsRecoveryDone = true;
        }

        public override void Exit()
        {
            IsRecoveryDone = false;
        }
    }
}

