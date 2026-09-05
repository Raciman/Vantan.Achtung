using Ach.FSM;

namespace Ach.Units.Player
{
    public sealed class LocomotionMachine: ILocomotionView
    {
        private readonly StateMachine _stateMachine;
        private readonly FreeState _freeState;
        private readonly DodgeState _dodgeState;
        
        public bool IsDead { get; private set; } //TODO _stateMachine.CurrentState == _dead
        public bool IsSprinting => _stateMachine.CurrentState == _freeState && 
                                   _freeState.IsSprinting;

        public LocomotionMachine(PlayerContext ctx)
        {
            _freeState = new FreeState(ctx);
            _dodgeState = new DodgeState(ctx);
            
            _stateMachine = new StateMachine();
            
            _stateMachine.SetInitialState(_freeState);
            _stateMachine.AddTransition(_freeState, _dodgeState, () => ctx.Intent.WantsDodge);
            _stateMachine.AddTransition(_dodgeState, _freeState, () => ctx.Intent.WantsAim); //TODO
        }
        
        public void Enter() => _stateMachine.Enter();
        public void Tick(float deltaTime) => _stateMachine.Tick(deltaTime);
    }
}


