using System;
using Ach.Input;
using UnityEngine;

namespace Ach.Units.Player
{
    public interface IPlayerIntent
    {
        Vector3 MoveDirection { get; }
        Vector3 LookDirection { get; }
        bool WantsAim { get; }
        bool WantsSprint { get; }
        bool WantsDodge { get; }
        bool WantsChangeWeapon { get; }
        bool InteractPressed { get; }
        
        int WeaponIndex { get; }
        bool FireHeld { get; }
        bool FirePressed { get; }
        bool ReloadPressed { get; }
        bool DropPressed { get; }


        void ConsumeFire();
        void ConsumeReload();
        void ConsumeInteract();
        void ConsumeChangeWeapon();
        void ConsumeDrop();
    }
    
    public sealed class PlayerIntentProvider : IPlayerIntent, IDisposable
    {
        private readonly IInputService _input;
        private readonly Transform _owner;
        
        public Vector3 MoveDirection { get; private set; }
        public Vector3 LookDirection { get; private set; }
        public bool WantsAim { get; private set; }
        public bool WantsSprint { get; private set; }
        public bool WantsChangeWeapon => Time.time - _switchAt <= BufferTimer;
        public int WeaponIndex { get; private set; }
        public bool FireHeld { get; private set; }
        public bool WantsDodge { get; private set; }
        public bool InteractPressed => Time.time - _interactAt <= BufferTimer;
        public bool FirePressed => Time.time - _fireAt <= BufferTimer;
        public bool ReloadPressed =>  Time.time - _reloadAt <= BufferTimer;
        public bool DropPressed =>  Time.time - _dropAt <= BufferTimer;
        private Camera _camera;

        private const float MinAimDistanceSqr = 0.25f;
        private const float BufferTimer = 0.2f;
        private PointerWorldProjector _projector;

        private float _fireAt = float.NegativeInfinity;
        private float _reloadAt = float.NegativeInfinity;
        private float _switchAt = float.NegativeInfinity;
        private float _interactAt = float.NegativeInfinity;
        private float _dropAt = float.NegativeInfinity;

        public PlayerIntentProvider(IInputService inputService, Transform owner, Camera camera)
        {
            
            _input = inputService;
            _owner = owner;
            _camera = camera;
            
            LookDirection = _owner.forward;
            
            _projector = new PointerWorldProjector(camera);

            _input.FirePressed += FireHandler;
            _input.WeaponSlotPressed += ChangeWeaponHandler;
            _input.ReloadPressed += ReloadHandler;
            _input.InteractPressed += InteractHandler;
            _input.DropPressed += DropHandler;
        }

        private void DropHandler() => _dropAt = Time.time;

        private void InteractHandler() => _interactAt = Time.time;

        private void ReloadHandler() => _reloadAt = Time.time;

        private void ChangeWeaponHandler(int index)
        {
            _switchAt = Time.time;
            WeaponIndex = index;
        }

        private void FireHandler() => _fireAt = Time.time;

        public void Tick(float deltaTime)
        {
            MoveDirection = new Vector3(_input.InputAxis.x, 0, _input.InputAxis.y);
            WantsAim = _input.AimHeld;
            WantsSprint = _input.SprintHeld;
            FireHeld = _input.FireHeld;
            if (_projector.TryProject(_input.MousePosition, out Vector3 worldPos, _owner.transform.position.y))
            {
                Vector3 dir = worldPos - _owner.position;
                dir.y = 0f;
                if(dir.sqrMagnitude > MinAimDistanceSqr)
                    LookDirection = dir.normalized;
            }
        }
        
        public void ConsumeFire() => _fireAt = float.NegativeInfinity;
        
        public void ConsumeReload() => _reloadAt = float.NegativeInfinity;
        
        public void ConsumeInteract() => _interactAt = float.NegativeInfinity;
        
        public void ConsumeChangeWeapon() => _switchAt = float.NegativeInfinity;

        public void ConsumeDrop() => _dropAt = float.NegativeInfinity;
        

        public void Dispose()
        {
            _input.FirePressed -= FireHandler;
            _input.WeaponSlotPressed -= ChangeWeaponHandler;
            _input.ReloadPressed -= ReloadHandler;
            _input.InteractPressed -= InteractHandler;
            _input.DropPressed -= DropHandler;

        }
    }
}

