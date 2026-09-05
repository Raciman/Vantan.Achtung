using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ach.Input
{
    public interface IInputService
    {
        Vector2 InputAxis { get; }
        Vector2 MousePosition { get; }
        public bool AimHeld { get; }
        public bool SprintHeld { get; }
        public event Action FirePressed;
        public event Action<int> WeaponSlotPressed;
        public event Action ReloadPressed;
    }
    
    public sealed class InputService : IInputService, IDisposable
    {
        private readonly InputSystem_Actions _actions;
        
        public event Action FirePressed;
        public event Action<int> WeaponSlotPressed;
        public event Action ReloadPressed;

        public InputService()
        {
            _actions = new InputSystem_Actions();
            _actions.Enable();
            _actions.Player.Enable();
            _actions.Player.Attack.performed += AttackPressed;
            _actions.Player.WeaponSlot1.performed += WeaponSlot1Pressed;
            _actions.Player.WeaponSlot2.performed += WeaponSlot2Pressed;
            _actions.Player.Reload.performed += ReloadButtonPressed;
        }

        private void ReloadButtonPressed(InputAction.CallbackContext obj)
            => ReloadPressed?.Invoke();

        private void WeaponSlot2Pressed(InputAction.CallbackContext obj)
            => WeaponSlotPressed?.Invoke(2);

        private void WeaponSlot1Pressed(InputAction.CallbackContext obj)
            => WeaponSlotPressed?.Invoke(1);


        private void AttackPressed(InputAction.CallbackContext obj)
            => FirePressed?.Invoke();


        public Vector2 InputAxis => _actions.Player.Move.ReadValue<Vector2>();
        public Vector2 MousePosition => _actions.Player.Look.ReadValue<Vector2>();

        public bool AimHeld => _actions.Player.Aim.IsPressed();
        public bool SprintHeld => _actions.Player.Sprint.IsPressed();

        public void Dispose()
        {
            
            _actions.Player.Disable();
            _actions.Disable();
            
            _actions.Player.Attack.performed -= AttackPressed;
            _actions.Player.WeaponSlot1.performed -= WeaponSlot1Pressed;
            _actions.Player.WeaponSlot2.performed -= WeaponSlot2Pressed;
            _actions.Player.Reload.performed -= ReloadButtonPressed;

            _actions?.Dispose();

        }
    }
}

