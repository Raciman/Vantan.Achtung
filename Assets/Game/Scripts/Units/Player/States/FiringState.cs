using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class FiringState : PlayerStateBase
    {
        private float _sinceShot;
        public bool IsCompleted { get ; private set; }

        public FiringState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            IsCompleted = false;
            Ctx.Intent.ConsumeFire();
            Shoot();
            
        }



        public override void Tick(float deltaTime)
        {
            _sinceShot += deltaTime;
            if(_sinceShot < Ctx.Weapon.FireInterval)
                return;

            if (Ctx.Weapon.IsAutoMode && Ctx.Intent.FireHeld && Ctx.Weapon.CanFire && Ctx.Intent.WantsAim)
                Shoot();
            else
                IsCompleted = true;
        }

        public override void Exit()
        {
            IsCompleted = false;
        }
        
        private void Shoot()
        {
            _sinceShot = 0f;
            Ctx.Weapon.Fire();
            Ctx.Animator.PlayFire(1);

        }
    }
}

