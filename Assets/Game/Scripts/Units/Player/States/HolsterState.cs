
namespace Ach.Units.Player
{
    public sealed class HolsterState : PlayerStateBase
    {
        private float _elapsed;
        public bool IsCompleted { get; private set; }
        
        public HolsterState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            _elapsed = 0;
            IsCompleted = false;

            int requested = Ctx.Intent.WeaponIndex;
            Ctx.Weapon.RequestSlot(requested);
            Ctx.Intent.ConsumeChangeWeapon();
            
            Ctx.Weapon.CancelReload();
            Ctx.Animator.PlayHolster();
        }

        public override void Tick(float deltaTime)
        {
            _elapsed += deltaTime;
            if(_elapsed >= Ctx.Config.HolsterDuration)
                IsCompleted = true;
        }

        public override void Exit()
        {
            IsCompleted = false;

            if (Ctx.Weapon.PendingSlot == 0)
            {
                Ctx.Weapon.ApplyPendingSlot();
                Ctx.Animator.SetWeaponLayer(0);
            }
            
            /*var index = Ctx.Intent.WeaponIndex;
            Ctx.Intent.ConsumeChangeWeapon();

            if (Ctx.Weapon.CurrentSlot == index)
                Ctx.Weapon.UnequipSlot();
            else
                Ctx.Weapon.EquipSlot(index);
            //Ctx.Animator.SetWeaponLayer(Ctx.Weapon.Current.LayerIndex); TODO
            Ctx.Animator.SetWeaponLayer(index);*/
        }
    }

}
