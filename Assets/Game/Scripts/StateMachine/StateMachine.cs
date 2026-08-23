
using System;
using System.Collections.Generic;

namespace Ach.FSM
{
    public sealed class StateMachine: IState
    {
        private static readonly List<Transition> EmptyTransitions = new (0);
        private const int MaxChainedTransitions = 5;
        
        private readonly Dictionary<IState, List<Transition>> _transitions = new ();
        private readonly List<Transition> _anyTransitions = new ();

        private IState _initialState;
        private IState _currentState;
        private List<Transition> _currentTransitions = EmptyTransitions;
        
        public IState CurrentState => _currentState;
        public event Action<IState, IState> StateChanged;
        
        public void SetInitialState(IState initialState) => _initialState = initialState;

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (!_transitions.TryGetValue(from, out var transitions))
            {
                transitions = new();
                _transitions[from] = transitions;
            }
            
            transitions.Add(new Transition(to, condition));
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
            => _anyTransitions.Add(new Transition(to, condition));
        

        public StateMachine(IState initState)
        {
            _currentState = initState;
            _currentState.Enter();
        }

        public void Enter() => ChangeState(_initialState);
        
        public void Tick(float deltaTime)
        {
            for (int i = 0; i < MaxChainedTransitions; i++)
            {
                if (!TryGetTransitions(out var nextState))
                    break;
                
                ChangeState(nextState);
            }
            
            _currentState?.Tick(deltaTime);
        }

        private bool TryGetTransitions(out IState nextState)
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.To != _currentState && transition.Condition())
                {
                    nextState = transition.To;
                    return true;
                }
            }

            foreach (var transition in _currentTransitions)
            {
                if (transition.Condition())
                {
                    nextState = transition.To;
                    return true;
                }
            }
            
            nextState = null;
            return false;
        }

        public void Exit()
        {
            _currentState?.Exit();
            _currentState = null;
            _currentTransitions = EmptyTransitions;
        }

        private void ChangeState(IState newState)
        {
            if (_currentState == newState)
                return;
            
            _currentState?.Exit();
            var previous = _currentState;
            _currentState = newState;
            _currentTransitions = _transitions.GetValueOrDefault(newState,  EmptyTransitions);
            _currentState.Enter();
            
            StateChanged?.Invoke(previous, _currentState);
        }
    }
}

