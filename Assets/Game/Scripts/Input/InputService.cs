using System;
using UnityEngine;

namespace Ach.Input
{
    public interface IInputService
    {
        Vector2 InputAxis { get; }
    }
    
    public class InputService : IInputService, IDisposable
    {
        private InputSystem_Actions _actions;

        public InputService()
        {
            _actions = new InputSystem_Actions();
            _actions.Enable();
            _actions.Player.Enable();
        }
        
        public Vector2 InputAxis => _actions.Player.Move.ReadValue<Vector2>();

        public void Dispose()
        {
            _actions.Player.Disable();
            _actions.Disable();
            _actions?.Dispose();
        }
    }
}

