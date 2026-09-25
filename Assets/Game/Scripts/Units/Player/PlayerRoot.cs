using Ach.Input;
using Reflex.Attributes;
using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class PlayerRoot : MonoBehaviour
    {
        [SerializeField] private PlayerConfigSO config;
        
        [SerializeField] private Camera camera;
        [SerializeField] private CharacterControllerMotor motor;
        [SerializeField] private LookController look;
        [SerializeField] private PlayerAnimatorController animator;
        [SerializeField] private TurnInPlaceController turnInPlace;
        [SerializeField] private WeaponHandler weaponHandler;
        [SerializeField] private InteractController interact;

        private PlayerIntentProvider _intent;
        private LocomotionMachine _locomotion;
        private StanceMachine _stance;
        

        [Inject] private IInputService _input;

        private void Awake()
        {
            _intent = new PlayerIntentProvider(_input, transform, camera);
            var ctx = new PlayerContext(_intent, motor, look, animator, weaponHandler, config, interact);

            _locomotion = new LocomotionMachine(ctx);
            _stance = new StanceMachine(ctx);
            
            animator.Init(_stance, _locomotion);
            weaponHandler.Init(_stance);
            
            ctx.BuildMachines(_stance,  _locomotion);
            
            _locomotion.Enter();
            _stance.Enter();
        }

        private void Update()
        {
            if (!_input.GameplayEnabled)
                return;

            float dt = Time.deltaTime;
            
            _intent.Tick(dt);
            
            weaponHandler.Tick(dt);
            
            _locomotion.Tick(dt);
            _stance.Tick(dt);
            
            
            motor.Tick(dt);
            look.Tick(dt);
            turnInPlace.Tick(dt);
            animator.Tick(dt);
        }

        private void OnDestroy()
        {
            _intent.Dispose();
        }
    }
}

