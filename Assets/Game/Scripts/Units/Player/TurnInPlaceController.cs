using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class TurnInPlaceController : MonoBehaviour
    {
        private static readonly int TurnAngle = Animator.StringToHash("TurnAngle");
        private static readonly int Turn = Animator.StringToHash("Turn");

        [SerializeField] private Animator animator;
        [SerializeField] private LookController look;
        [SerializeField] private CharacterControllerMotor motor;

        [SerializeField] private float triggerAngle = 40f;
        [SerializeField] private float releaseAngle = 13f;
        [SerializeField] private float moveThreshold = 0.1f;
        [SerializeField] private float interval = 0.25f;

        private float _cooldown;
        private bool _isTurning;

        public void Tick(float deltaTime)
        {
            if(_cooldown > 0f)
                _cooldown -= deltaTime;

            if (motor.PlanarVelocity.sqrMagnitude > moveThreshold * moveThreshold)
            {
                _isTurning = false;
                return;
            }

            var signed = look.SignedAngleToTarget;
            var magnitude = Mathf.Abs(signed);

            if (_isTurning)
            {
                if(magnitude < releaseAngle)
                    _isTurning = false;
                
                return;
            }

            if (magnitude < triggerAngle || _cooldown > 0f)
                return;
            
            animator.SetFloat(TurnAngle, signed);
            animator.SetTrigger(Turn);
            
            _isTurning = true;
            _cooldown = interval;
        }
    }
}

