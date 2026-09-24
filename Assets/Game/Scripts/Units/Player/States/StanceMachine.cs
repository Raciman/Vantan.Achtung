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
        private readonly InteractState _interact;
        private readonly DropState _drop;
        
        public bool IsAiming => _stateMachine.CurrentState == _aim;
        public bool IsReloading => _stateMachine.CurrentState == _reload;
        public bool IsWeaponRaised =>  _stateMachine.CurrentState == _aim ||
                                       _stateMachine.CurrentState == _firing;


        public StanceMachine(PlayerContext ctx)
        {
            _stateMachine = new StateMachine();
            _aim = new AimState(ctx);
            _idle = new IdleState(ctx);
            _firing = new FiringState(ctx);
            _reload = new ReloadState(ctx);
            _holster = new HolsterState(ctx);
            _draw = new DrawState(ctx);
            _interact = new InteractState(ctx);
            _drop =  new DropState(ctx);
            
            //Stance
            _stateMachine.AddTransition(_idle, _aim, () 
                => ctx.Intent.WantsAim && ctx.Weapon.HasWeapon);
            _stateMachine.AddTransition(_aim, _idle, () 
                => !ctx.Intent.WantsAim);
            _stateMachine.AddTransition(_idle, _interact, ()
                => ctx.Intent.InteractPressed);
            _stateMachine.AddTransition(_interact, _idle, () 
                => _interact.IsCompleted);
            _stateMachine.AddTransition(_idle, _drop, ()
                => ctx.Intent.DropPressed && ctx.Weapon.HasWeapon);
            _stateMachine.AddTransition(_drop, _idle, () 
                => _drop.IsCompleted);
            
            //Attack
            _stateMachine.AddTransition(_aim, _firing, () 
                => ctx.Weapon.CanFire && 
                   (ctx.Intent.FirePressed || (ctx.Intent.FireHeld && ctx.Weapon.IsAutoMode)));
            _stateMachine.AddTransition(_firing, _aim, () 
                =>_firing.IsCompleted && ctx.Intent.WantsAim);
            _stateMachine.AddTransition(_firing, _idle, () 
                =>_firing.IsCompleted && !ctx.Intent.WantsAim);
            
            //Reload
            _stateMachine.AddTransition(_idle, _reload, () 
                => ctx.Intent.ReloadPressed && ctx.Weapon.CanReload);
            _stateMachine.AddTransition(_reload, _idle, () 
                => _reload.IsCompleted);
            
            //WeaponChange Holster
            _stateMachine.AddTransition(_idle, _holster, () 
                => ctx.Intent.WantsChangeWeapon && ctx.Weapon.HasWeapon 
                                                && ctx.Weapon.CanSelect(ctx.Intent.WeaponIndex));
            _stateMachine.AddTransition(_aim, _holster, () 
                => ctx.Intent.WantsChangeWeapon && ctx.Weapon.HasWeapon
                                                && ctx.Weapon.CanSelect(ctx.Intent.WeaponIndex));
            _stateMachine.AddTransition(_reload, _holster, () 
                => ctx.Intent.WantsChangeWeapon && ctx.Weapon.HasWeapon
                                                && ctx.Weapon.CanSelect(ctx.Intent.WeaponIndex));
            _stateMachine.AddTransition(_idle, _draw, () 
                => ctx.Intent.WantsChangeWeapon && !ctx.Weapon.HasWeapon
                                                && ctx.Weapon.CanSelect(ctx.Intent.WeaponIndex));
            
            //WeaponChange Draw
            _stateMachine.AddTransition(_holster, _draw, () 
                => _holster.IsCompleted && ctx.Weapon.HasPending);
            _stateMachine.AddTransition(_holster, _idle, () 
                => _holster.IsCompleted && !ctx.Weapon.HasPending);
            _stateMachine.AddTransition(_draw, _idle, () 
                => _draw.IsCompleted);

            
            _stateMachine.SetInitialState(_idle);
        }

        public void Enter() => _stateMachine.Enter();

        public void Tick(float deltaTime) => _stateMachine.Tick(deltaTime);
    }

}
