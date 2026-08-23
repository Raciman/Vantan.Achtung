using System;
using Ach.FSM;
using Ach.Input;
using Reflex.Attributes;
using UnityEngine;

namespace Units.Player
{
    public class PlayerRoot : MonoBehaviour
    {
        [SerializeField] private CharacterControllerMotor motor;

        private PlayerIntentProvider _intent;
        private StateMachine _stateMachine;

        [Inject] private IInputService _input;

        private void Awake()
        {
            var ctx = new PlayerContext(motor);
            _intent = new PlayerIntentProvider(_input, transform);
            var freeState = new FreeState(ctx, motor, _intent);

            _stateMachine = new StateMachine(freeState);
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            
            _intent.Tick(dt);
            _stateMachine.Tick(dt);
            motor.Tick(dt);
        }
    }

    public class PlayerContext
    {
        public readonly CharacterControllerMotor Motor;
    
        public PlayerContext(CharacterControllerMotor motor)
        {
            Motor = motor;
        }
    }
}

