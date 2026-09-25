using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

namespace Ach.Input
{
    public interface IInputService
    {
        Vector2 InputAxis { get; }
        Vector2 MousePosition { get; }
        public bool AimHeld { get; }
        public bool SprintHeld { get; }
        public bool FireHeld { get; }
        public event Action FirePressed;
        public event Action<int> WeaponSlotPressed;
        public event Action ReloadPressed;
        public event Action InteractPressed;
        public event Action DropPressed;
        bool GameplayEnabled { get; }
        event Action PausePressed;
        event Action GameplayInputDisabled;
        void SetGameplayEnabled(bool enabled);
    }
    
    public sealed class InputService : IInputService, IDisposable
    {
        private readonly InputSystem_Actions _actions;
        private bool _fireRequiresRelease;
        private int _enabledFrame = -1;
        public bool GameplayEnabled { get; private set; } = true;
        public event Action PausePressed;
        public event Action GameplayInputDisabled;
        
        public event Action FirePressed;
        public event Action<int> WeaponSlotPressed;
        public event Action ReloadPressed;
        public event Action InteractPressed;
        public event Action DropPressed;

        public InputService()
        {
            _actions = new InputSystem_Actions();
            _actions.Enable();
            _actions.Player.Enable();
            _actions.Player.Attack.performed += AttackPressed;
            _actions.Player.WeaponSlot1.performed += WeaponSlot1Pressed;
            _actions.Player.WeaponSlot2.performed += WeaponSlot2Pressed;
            _actions.Player.Reload.performed += ReloadButtonPressed;
            _actions.Player.Interact.performed += InteractButtonPressed;
            _actions.Player.DropItem.performed += DropButtonPressed;
            _actions.UI.Pause.performed += PauseButtonPressed;
            InputSystem.onAfterUpdate += CheckFireReleased;
        }

        public void SetGameplayEnabled(bool enabled)
        {
            if (GameplayEnabled == enabled)
                return;
            GameplayEnabled = enabled;
            _fireRequiresRelease = true;
            if (enabled)
            {
                _enabledFrame = Time.frameCount;
                _actions.Player.Enable();
            }
            else
            {
                _actions.Player.Disable();
                GameplayInputDisabled?.Invoke();
            }
        }

        private void CheckFireReleased()
        {
#if UNITY_EDITOR
            // Editor updates read a separate device buffer; only gameplay can release this latch.
            if (InputState.currentUpdateType == InputUpdateType.Editor)
                return;
#endif
            // Neither a held trigger nor the click closing a menu should become a shot.
            if (!GameplayEnabled || !_fireRequiresRelease || Time.frameCount <= _enabledFrame)
                return;
            foreach (var control in _actions.Player.Attack.controls)
                if (control is ButtonControl button && button.isPressed)
                    return;
            _fireRequiresRelease = false;
        }

        private void PauseButtonPressed(InputAction.CallbackContext _) => PausePressed?.Invoke();

        private void DropButtonPressed(InputAction.CallbackContext obj)
            => DropPressed?.Invoke();

        private void InteractButtonPressed(InputAction.CallbackContext obj)
            => InteractPressed?.Invoke();
        
        private void ReloadButtonPressed(InputAction.CallbackContext obj)
            => ReloadPressed?.Invoke();

        private void WeaponSlot2Pressed(InputAction.CallbackContext obj)
            => WeaponSlotPressed?.Invoke(1);

        private void WeaponSlot1Pressed(InputAction.CallbackContext obj)
            => WeaponSlotPressed?.Invoke(0);
        
        private void AttackPressed(InputAction.CallbackContext obj)
        {
            if (GameplayEnabled && !_fireRequiresRelease)
                FirePressed?.Invoke();
        }


        public Vector2 InputAxis => _actions.Player.Move.ReadValue<Vector2>();
        public Vector2 MousePosition => _actions.Player.Look.ReadValue<Vector2>();

        public bool AimHeld => _actions.Player.Aim.IsPressed();
        public bool SprintHeld => _actions.Player.Sprint.IsPressed();
        public bool FireHeld => GameplayEnabled && !_fireRequiresRelease && _actions.Player.Attack.IsPressed();

        public void Dispose()
        {
            InputSystem.onAfterUpdate -= CheckFireReleased;
            _actions.UI.Pause.performed -= PauseButtonPressed;
            
            _actions.Player.Disable();
            _actions.Disable();
            
            _actions.Player.Attack.performed -= AttackPressed;
            _actions.Player.WeaponSlot1.performed -= WeaponSlot1Pressed;
            _actions.Player.WeaponSlot2.performed -= WeaponSlot2Pressed;
            _actions.Player.Reload.performed -= ReloadButtonPressed;
            _actions.Player.Interact.performed -= InteractButtonPressed;
            _actions.Player.DropItem.performed -= DropButtonPressed;

            
            _actions?.Dispose();

        }
    }
}

