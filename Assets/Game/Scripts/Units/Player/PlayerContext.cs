
using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class PlayerContext
    {
        public IPlayerIntent Intent { get; }
        public CharacterControllerMotor Motor { get; }
        public LookController Look { get; }
        public PlayerAnimatorController Animator { get; }
        public WeaponHandler Weapon { get; }
    
        public IStanceView Stance { get; private set; }
        public ILocomotionView Locomotion { get; private set; }
        
        public PlayerConfigSO Config { get; private set; }
        
        
        public PlayerContext(IPlayerIntent intent, CharacterControllerMotor motor, 
            LookController look, PlayerAnimatorController animator, WeaponHandler weaponHandler, 
            PlayerConfigSO config)
        {
            Intent = intent;
            Motor = motor;
            Look = look;
            Animator = animator;
            Weapon = weaponHandler;
            Config = config;
        }

        public void BuildMachines(IStanceView stance, ILocomotionView locomotion)
        {
            Stance = stance;
            Locomotion = locomotion;
        }
    }
}

