
using RPool;
using UnityEngine;
using UnityEngine.AI;

namespace Ach.Units.Enemies
{
    public class EnemyRoot : MonoBehaviour, IPoolable
    {
       
        [SerializeField] private EnemyConfigSO config;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private HealthComponent health;
        [SerializeField] private RagdollController ragdoll;

        [SerializeField] private EnemyAnimatorController animator;
        
        private EnemyContext _context;
        private EnemyIntentProvider  _intent;
        private EnemyBehaviourMachine _behaviourMachine;

        public void Init(IDamageable player)
        {
            
            _intent = new EnemyIntentProvider(transform, player.Transform, obstacleMask);
            _context = new EnemyContext(_intent, config, transform.position, agent, player, animator,
                health, ragdoll);
            
            _behaviourMachine = new EnemyBehaviourMachine(_context);
            
            _behaviourMachine.Enter();
        }
        
        private void Awake()
        {

        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _intent.Tick(dt);
            _behaviourMachine.Tick(dt);
            
            animator.Tick(dt);
        }

        public IPool Owner { get; set; }
        public void OnGet()
        {
            
        }

        public void OnRelease()
        {
            
        }
    }
}

