using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class LookController : MonoBehaviour
    {
        
        [SerializeField] private PlayerConfigSO config;

        private Vector3 _targetDirection;
        private bool _hasTarget;

        public float SignedAngleToTarget
            => Vector3.SignedAngle(transform.forward, _targetDirection, Vector3.up);

        public bool IsAligned(float toleranceDeg)
            => Mathf.Abs(SignedAngleToTarget) < toleranceDeg;
        
        
        public void SetTarget(Vector3 direction)
        {
            if(direction.sqrMagnitude < 0.001f)
                return;
            
            _targetDirection = direction;
            _hasTarget = true;
        }

        private void Awake()
        {
            _targetDirection = transform.forward;
        }

        public void Tick(float deltaTime)
        {
            RotateTowards(deltaTime);
        }

        private void RotateTowards(float deltaTime)
        {
            if(!_hasTarget)
                return;
            
            var target = Quaternion.LookRotation(_targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                target, config.RotateSpeed * deltaTime);

            _hasTarget = false;
        }
    }
}

