using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class PlayerAnimatorController : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMove");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int IsAiming = Animator.StringToHash("IsAiming");
        private static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
        private static readonly int Fire = Animator.StringToHash("Fire");
        private static readonly int Reload = Animator.StringToHash("Reload");
        private static readonly int Holster = Animator.StringToHash("Holster");
        private static readonly int Draw = Animator.StringToHash("Draw");

        [SerializeField] private Animator animator;
        [SerializeField] private CharacterControllerMotor motor;
        
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float dampTime = 0.1f;
        [SerializeField] private float moveThreshold = 0.05f;


        [SerializeField] private float layerBlendTime = 0.13f;
        private const int WeaponLayerCount = 2;
        private int _targetLayer;
        private int _currentLayer;
        
        private IStanceView _stanceView;
        private ILocomotionView _locomotionView;

        public void Init(IStanceView stanceView, ILocomotionView locomotionView)
        {
            _stanceView = stanceView;
            _locomotionView = locomotionView;
        }
        
        public void Tick(float deltaTime)
        {
            var local = transform.InverseTransformDirection(motor.PlanarVelocity) / motor.MaxSpeed;

            
            animator.SetBool(IsMoving, local.sqrMagnitude > moveThreshold * moveThreshold);
            animator.SetFloat(Horizontal, local.x, dampTime, deltaTime);
            animator.SetFloat(Vertical,   local.z, dampTime, deltaTime);
            animator.SetBool(IsSprinting, _locomotionView.IsSprinting);
            animator.SetBool(IsAiming, _stanceView.IsAiming);

            for (int i = 1; i <= WeaponLayerCount; i++)
            {
                float target = (i == _targetLayer) ? 1f : 0f;
                animator.SetLayerWeight(i,
                    Mathf.MoveTowards(animator.GetLayerWeight(i), target, deltaTime / layerBlendTime));
            }
        }

        public void PlayFire()
        {
            animator.SetTrigger(Fire);
        }

        public void SetWeaponLayer(int weaponIndex)
        {
            _targetLayer = weaponIndex;

            /*_currentLayer = weaponIndex;
            for (int i = 1; i <= 2; i++)
            {
                animator.SetLayerWeight(i, 0f);
            }

            animator.SetLayerWeight(weaponIndex, 1f);*/
        }

        public void PlayReload()
        {
            animator.SetTrigger(Reload);
        }

        public void PlayHolster()
        {
            animator.SetTrigger(Holster);
        }

        public void PlayDraw()
        {
            animator.SetTrigger(Draw);
        }

        /*public bool IsStateFinished()
        {
            
            if(_currentLayer == 0)
                return true;
            var info = animator.GetCurrentAnimatorStateInfo(_currentLayer);
            return !animator.IsInTransition(_currentLayer) && info.normalizedTime >= 1f;
        }*/
    }
}

