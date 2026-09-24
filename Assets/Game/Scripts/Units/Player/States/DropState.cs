namespace Ach.Units.Player
{
    public class DropState : PlayerStateBase
    {
        
        public bool IsCompleted { get; private set; }
        public DropState(PlayerContext ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            Ctx.Intent.ConsumeDrop();
            Ctx.Weapon.DropWeapon();
            IsCompleted = false;
        }

        public override void Tick(float deltaTime)
        {
            IsCompleted = true;
        }

        public override void Exit()
        {
            Ctx.Animator.SetWeaponLayer(0);
            IsCompleted = false;
        }
    }
}