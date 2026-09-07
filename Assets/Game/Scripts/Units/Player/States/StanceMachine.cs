using Ach.FSM;

namespace Ach.Units.Player
{
    public sealed class StanceMachine : IStanceView
    {
        private readonly StateMachine _stateMachine;
        private readonly AimState _aim;
        private readonly IdleState _idle;
        private readonly FiringState _firing;
        private readonly ReloadState _reload;
        private readonly HolsterState _holster;
        private readonly DrawState _draw;
        
        public bool IsAiming => _stateMachine.CurrentState == _aim;
        public bool IsReloading => _stateMachine.CurrentState == _reload;
        
        
        public StanceMachine(PlayerContext ctx)
        {
            _stateMachine = new StateMachine();
            _aim = new AimState(ctx);
            _idle = new IdleState(ctx);
            _firing = new FiringState(ctx);
            _reload = new ReloadState(ctx);
            _holster = new HolsterState(ctx);
            _draw = new DrawState(ctx);
            
            //Stance
            _stateMachine.AddTransition(_idle, _aim, () 
                => ctx.Intent.WantsAim);
            _stateMachine.AddTransition(_aim, _idle, () 
                => !ctx.Intent.WantsAim);
            
            //Attack
            _stateMachine.AddTransition(_aim, _firing, () 
                => ctx.Intent.FirePressed && ctx.Weapon.CanFire);
            _stateMachine.AddTransition(_firing, _aim, () 
                =>_firing.IsRecoveryDone && ctx.Intent.WantsAim);
            _stateMachine.AddTransition(_firing, _idle, () 
                =>_firing.IsRecoveryDone && !ctx.Intent.WantsAim);
            
            //Reload
            _stateMachine.AddTransition(_idle, _reload, () 
                => ctx.Intent.ReloadPressed && ctx.Weapon.CanReload);
            _stateMachine.AddTransition(_reload, _idle, () 
                => _reload.IsCompleted);
            
            //WeaponChange Holster
            _stateMachine.AddTransition(_idle, _holster, () 
                => ctx.Intent.WantsChangeWeapon && ctx.Weapon.HasWeapon);
            _stateMachine.AddTransition(_aim, _holster, () 
                => ctx.Intent.WantsChangeWeapon && ctx.Weapon.HasWeapon);
            _stateMachine.AddTransition(_reload, _holster, () 
                => ctx.Intent.WantsChangeWeapon && ctx.Weapon.HasWeapon);
            _stateMachine.AddTransition(_idle, _draw, () 
                => ctx.Intent.WantsChangeWeapon && !ctx.Weapon.HasWeapon);
            
            //WeaponChange Draw
            _stateMachine.AddTransition(_holster, _draw, () 
                => _holster.IsCompleted && ctx.Weapon.PendingSlot != 0);
            _stateMachine.AddTransition(_holster, _idle, () 
                => _holster.IsCompleted && ctx.Weapon.PendingSlot == 0);
            _stateMachine.AddTransition(_draw, _idle, () 
                => _draw.IsCompleted);

            
            _stateMachine.SetInitialState(_idle);
        }

        public void Enter() => _stateMachine.Enter();

        public void Tick(float deltaTime) => _stateMachine.Tick(deltaTime);
    }

}
