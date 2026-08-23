using System;
using Ach.Input;
using UnityEngine;

namespace Units.Player
{
    public interface IPlayerIntent
    {
        Vector3 MoveDirection { get; }
        Vector3 LookDirection { get; }
        bool WantsAim { get; }
        
        bool FirePressed { get; }
        bool ReloadPressed { get; }
        bool InteractPressed { get; }

        void ConsumeFire();
        void ConsumeReload();
        void ConsumeInteract();
    }
    
    public class PlayerIntentProvider : IPlayerIntent, IDisposable
    {
        private readonly IInputService _input;
        private readonly Transform _owner;
        
        public Vector3 MoveDirection { get; private set; }
        public Vector3 LookDirection { get; private set; }
        public bool WantsAim { get; private set; }
        public bool FirePressed { get; private set; }
        public bool ReloadPressed { get;  private set; }
        public bool InteractPressed { get;  private set; }

        public PlayerIntentProvider(IInputService inputService, Transform owner)
        {
            _input = inputService;
            _owner = owner;
            
            LookDirection = _owner.forward;
        }

        public void Tick(float deltaTime)
        {
            Vector3 dir = new Vector3(_input.InputAxis.x, 0, _input.InputAxis.y);
            MoveDirection = dir;
        }
        
        public void ConsumeFire()
        {
            
        }

        public void ConsumeReload()
        {
            
        }

        public void ConsumeInteract()
        {
            
        }

        public void Dispose()
        {
            
            //Отписки
        }
    }
}

