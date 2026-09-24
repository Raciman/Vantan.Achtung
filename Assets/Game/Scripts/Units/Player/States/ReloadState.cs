using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class ReloadState : PlayerStateBase
    {
        private float _elapsed;
        public bool IsCompleted { get; private set; }
        public ReloadState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            _elapsed = 0;
            IsCompleted = false;
            Ctx.Weapon.StartReload();
            Ctx.Animator.PlayReload(Ctx.Weapon.ReloadDuration);
            Ctx.Intent.ConsumeReload();
        }

        public override void Tick(float deltaTime)
        {
            _elapsed += deltaTime;
            if (!IsCompleted && _elapsed >= Ctx.Weapon.ReloadDuration)
            {
                Ctx.Weapon.CompleteReload();
                IsCompleted = true;
            }
        }

        public override void Exit()
        {
            IsCompleted = false;

        }
    }
}

