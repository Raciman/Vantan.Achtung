using System;
using UnityEngine;

namespace Ach.Input
{
    public class InputService : IDisposable
    {

        private InputSystem_Actions _actions;

        public InputService()
        {
            _actions = new InputSystem_Actions();
            _actions.Enable();
            _actions.Player.Enable();
        }
        
        public Vector2 MoveInput => _actions.Player.Move.ReadValue<Vector2>();

        public void Dispose()
        {
            _actions.Player.Disable();
            _actions.Disable();
            _actions?.Dispose();
        }
    }
}

