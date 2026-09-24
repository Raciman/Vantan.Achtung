using UnityEngine;
using UnityEngine.AI;

namespace Ach.Units.Enemies
{
    public class EnemyContext
    {
        public IEnemyIntent Intent { get; }
        public EnemyConfigSO Config { get; }
        public Vector3 HomePosition { get; }
        public NavMeshAgent Agent { get; }
        public IDamageable Target { get; }
        public EnemyAnimatorController Animator { get; }
        public HealthComponent Health { get; }
        public RagdollController Ragdoll { get; }

        public EnemyContext(IEnemyIntent intent, EnemyConfigSO config, Vector3 position,
            NavMeshAgent agent, IDamageable target, EnemyAnimatorController  animator,
            HealthComponent health, RagdollController ragdoll)
        {
            Intent = intent;
            Config = config;
            HomePosition = position;
            Agent = agent;
            Target = target;
            Animator = animator;
            Health = health;
            Ragdoll = ragdoll;
        }
    }
}