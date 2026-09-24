using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class DrawState : PlayerStateBase
    {
        private float _elapsed;
        public bool IsCompleted { get; private set; }
        
        public DrawState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            _elapsed = 0f;
            IsCompleted = false;

            if (!Ctx.Weapon.HasPending && Ctx.Intent.WantsChangeWeapon)
                Ctx.Weapon.RequestSlot(Ctx.Intent.WeaponIndex);
            
            Ctx.Intent.ConsumeChangeWeapon();
            Ctx.Weapon.ApplyPendingWeapon();
            Ctx.Animator.SetWeaponLayer(Ctx.Weapon.CurrentWeapon.AnimLayer);
            Ctx.Animator.PlayDraw();
        }

        public override void Tick(float deltaTime)
        {
            _elapsed += deltaTime;
            if(_elapsed >= Ctx.Config.DrawDuration)
                IsCompleted = true;
        }

        public override void Exit()
        {
            IsCompleted = false;
        }
    }
}

