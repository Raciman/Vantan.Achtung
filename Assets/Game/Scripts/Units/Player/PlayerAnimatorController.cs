using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
        private static readonly int ActionSpeed = Animator.StringToHash("ActionSpeed");

        [SerializeField] private WeaponAnimSet[] weaponSets;

        [SerializeField] private Rig aimRig;
        [SerializeField] private float rigBlendTime = 0.15f;
        
        [SerializeField] private PlayerConfigSO config;
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterControllerMotor motor;
        
        [SerializeField] private float maxSpeed;
        [SerializeField] private float dampTime = 0.1f;
        [SerializeField] private float moveThreshold = 0.05f;

        [SerializeField] private float blendTime = 0.13f;

        [SerializeField] private float layerBlendTime = 0.13f;
        private const int WeaponLayerCount = 2;
        private int _targetLayer;
        private bool _justRequestedCrossFade;

        private WeaponAnimSet _set;
        private int _expectedState;
        
        private IStanceView _stanceView;
        private ILocomotionView _locomotionView;
        
        private bool CanPlayLayerAnimation => _set != null;

        public void Init(IStanceView stanceView, ILocomotionView locomotionView)
        {
            _stanceView = stanceView;
            _locomotionView = locomotionView;
            foreach (var set in weaponSets)
            {
                set.BuildHashes(animator);
            }
            
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

            if(_justRequestedCrossFade)
                _justRequestedCrossFade = false;
            else if (CanPlayLayerAnimation && _expectedState != 0 && !animator.IsInTransition(_set.Layer))
            {
                var info = animator.GetCurrentAnimatorStateInfo(_set.Layer);
                if (info.fullPathHash != _expectedState)
                    animator.Play(_expectedState, _set.Layer, 0f);   
                
            }
            
            float rigTarget = _stanceView.IsWeaponRaised ?  1f : 0f;
            aimRig.weight = Mathf.MoveTowards(aimRig.weight, rigTarget, deltaTime / rigBlendTime);
        }
        

        public void SetWeaponLayer(int slot)
        {
            _set = weaponSets.FirstOrDefault(t => t.Layer == slot);
            
            _targetLayer = _set?.Layer ?? 0;   
        }
        
        private void CrossFade(int stateHash, float logicDuration, float clipLength)
        {
            animator.SetFloat(ActionSpeed,
                logicDuration > 0.0001f && clipLength > 0.0001f ? clipLength / logicDuration : 1f);
            
            _expectedState = stateHash;
            _justRequestedCrossFade = true;
            animator.CrossFadeInFixedTime(stateHash, blendTime, _set.Layer, 0f);
        }

        public void PlayFire(float duration)
        {
            if (!CanPlayLayerAnimation) return;
            CrossFade(_set.Fire, duration, _set.FireLength);
        }

        public void PlayReload(float duration)
        {
            if (!CanPlayLayerAnimation) return;
            CrossFade(_set.Reload, duration, _set.ReloadLength);
        }

        public void PlayHolster()
        {
            if (!CanPlayLayerAnimation) return;
            CrossFade(_set.Holster, config.HolsterDuration, _set.HolsterLength);
        }

        public void PlayDraw()
        {
            if (!CanPlayLayerAnimation) return;
            CrossFade(_set.Draw, config.DrawDuration, _set.DrawLength);
        }

        public void PlayStanceIdle()
        {
            if (!CanPlayLayerAnimation) return;
            CrossFade(_set.Idle,0f, 0f);
        }

        public void PlayStanceAim()
        {
            if (!CanPlayLayerAnimation) return;
            CrossFade(_set.Aim, 0f, 0f);
        }

        public void SwitchAnimator(bool active) => animator.enabled = active;
    }
}

