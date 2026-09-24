
using UnityEngine;
using UnityEngine.AI;

namespace Ach.Units.Enemies
{
    public class EnemyRoot : MonoBehaviour
    {
        [SerializeField] private Transform player; //TODO Временно. Заменить на инит в пуле или ДИ
        
        [SerializeField] private EnemyConfigSO config;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private LayerMask obstacleMask;

        [SerializeField] private EnemyAnimatorController animator;
        
        private EnemyContext _context;
        private EnemyIntentProvider  _intent;
        private EnemyBehaviourMachine _behaviourMachine;

        private void Awake()
        {
            var target = player.GetComponent<HealthComponent>(); // TODO
            
            _intent = new EnemyIntentProvider(transform, player, obstacleMask);
            _context = new EnemyContext(_intent, config, transform.position, agent, target, animator);
            
            _behaviourMachine = new EnemyBehaviourMachine(_context);
            
            _behaviourMachine.Enter();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _intent.Tick(dt);
            _behaviourMachine.Tick(dt);
            
            animator.Tick(dt);
        }
    }
}

