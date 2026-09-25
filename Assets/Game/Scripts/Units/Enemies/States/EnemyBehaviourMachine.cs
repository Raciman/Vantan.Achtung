using Ach.FSM;
using UnityEngine;

namespace Ach.Units.Enemies
{
    public sealed class EnemyBehaviourMachine
    {
    
        private StateMachine _stateMachine;
        private IdleState _idleState;
        private WanderState _wanderState;
        private ChaseState _chaseState;
        private AttackState _attackState;
        private SeekState _seekState;
        private DeadState _deadState;
        

        public EnemyBehaviourMachine(EnemyContext context)
        {
            _idleState = new IdleState(context);
            _wanderState = new WanderState(context);
            _chaseState = new ChaseState(context);
            _attackState = new AttackState(context);
            _seekState = new SeekState(context);
            _deadState = new DeadState(context);

            _stateMachine = new StateMachine();
            
            _stateMachine.SetInitialState(_idleState);

            bool SeesTarget()
                => context.Intent.DistanceToTarget < context.Config.TargetDetectRadius
                   && context.Intent.CanSeeTarget && !context.Target.IsDead;
            bool ShouldChase() => SeesTarget() || context.Intent.WasHit;
            
            //Wander
            _stateMachine.AddTransition(_idleState, _wanderState, () => _idleState.IsCompleted);
            _stateMachine.AddTransition(_wanderState, _idleState, () => _wanderState.IsCompleted);
            
            //Chase
            _stateMachine.AddTransition(_chaseState, _idleState, () => context.Target.IsDead);  
            _stateMachine.AddTransition(_idleState, _chaseState, ShouldChase);
            _stateMachine.AddTransition(_wanderState, _chaseState, ShouldChase);
            _stateMachine.AddTransition(_seekState, _chaseState, ShouldChase);
            _stateMachine.AddTransition(_chaseState, _seekState, () => 
                context.Intent.DistanceToTarget > context.Config.TargetLoseRadius
                || context.Intent.HasLostTarget);
            
            //Attack
            _stateMachine.AddTransition(_chaseState, _attackState, () =>
                context.Intent.DistanceToTarget < context.Config.AttackRange
                && Time.time - _attackState.LastAttackTime >= context.Config.AttackCooldown 
                && !context.Target.IsDead);
            _stateMachine.AddTransition(_attackState, _chaseState, () =>
                _attackState.IsCompleted);
            
            //Seek
            _stateMachine.AddTransition(_seekState, _idleState, () =>
                _seekState.IsCompleted);
            
            //Dead
            _stateMachine.AddAnyTransition(_deadState, () => context.Health.IsDead);

        }

        public void Enter() => _stateMachine.Enter();
        public void Tick(float deltaTime) => _stateMachine.Tick(deltaTime);
    }
}

