using UnityEngine;
using UnityEngine.AI;

namespace Ach.Units.Enemies
{
    public class EnemyAnimatorController : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int ActionSpeed = Animator.StringToHash("ActionSpeed");

        [SerializeField] private EnemyConfigSO config;
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        
        [SerializeField] private EnemyAnimSet attackSet;

        public void Init()
        {
            
        }
        
        public void Tick(float deltaTime)
        {
            bool isMoving  = agent.velocity.magnitude >= 0.1f;
            animator.SetBool(IsMoving, isMoving);
        }

        public void PlayAttack()
        {
            animator.SetFloat(ActionSpeed,
                config.AttackDuration > 0.0001f && attackSet.Length > 0.0001f ? attackSet.Length / config.AttackDuration : 1f);
            
            animator.SetTrigger(Attack);
        }
    }
}

