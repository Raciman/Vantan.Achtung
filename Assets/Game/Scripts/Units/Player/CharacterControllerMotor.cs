using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class CharacterControllerMotor : MonoBehaviour
    {
        [SerializeField] private PlayerConfigSO config;
        
        [SerializeField] private CharacterController controller;

        private float _maxSpeed;
        private Vector3 _moveIntent;
        private Vector3 _planarVelocity;
        private Vector3 _impulse;
        private Vector3 _overrideVelocity;
        private bool _hasOverride;
        private float _speedMultiplier = 1f;
        private float _verticalVelocity = -1f;
        
        public float MaxSpeed => _maxSpeed;
        public void SetMoveIntent(Vector3 dir) => _moveIntent = Vector3.ClampMagnitude(dir, 1f);
        public void AddImpulse(Vector3 impulse) => _impulse += impulse;
        public Vector3 PlanarVelocity => _planarVelocity;

        private void Awake()
        {
            _maxSpeed = config.Speed;
        }

        public void SetOverrideVelocity(Vector3 velocity)
        {
            _overrideVelocity = velocity;
            _hasOverride = true;
        } 
        
        public void SetSpeedMultiplier(float speedMultiplier)
            => _speedMultiplier = speedMultiplier;
        
        public void Tick(float deltaTime)
        {
            if (_hasOverride)
            {
                _planarVelocity = _overrideVelocity;
                _hasOverride = false;
            }
            else
            {
                var target = _moveIntent * (_maxSpeed * _speedMultiplier);
                if (_moveIntent.sqrMagnitude > 0.001f)
                {
                    var currentDir = _planarVelocity.sqrMagnitude > 0.001f
                        ? _planarVelocity.normalized
                        : target.normalized;
                
                    var newDir = Vector3.RotateTowards(
                        currentDir, target.normalized,
                        config.TurnRate * Mathf.Deg2Rad * deltaTime, 0f);
                
                    var newSpeed = Mathf.MoveTowards(
                        _planarVelocity.magnitude, target.magnitude,
                        config.Acceleration * deltaTime);
                
                    _planarVelocity = newDir * newSpeed;
                }
                else
                {
                    _planarVelocity = Vector3.MoveTowards(
                        _planarVelocity, Vector3.zero, config.Deceleration * deltaTime);
                }
            }

            _impulse = Vector3.MoveTowards(_impulse, Vector3.zero, config.ImpulseDamping * deltaTime);
            
            if (controller.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = -1f;
            else
                _verticalVelocity -= config.Gravity * deltaTime;
            
            var motion = (_planarVelocity + _impulse) * deltaTime;
            motion.y = _verticalVelocity * deltaTime;
            controller.Move(motion);
            
        }
    }
}

