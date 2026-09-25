using Ach.Events;
using UnityEngine;

namespace Ach.Units.Player
{
    public class PlayerHealthComponent : HealthComponent
    {
        [SerializeField] private IntIntEvent healthChangedEvent;
        [SerializeField] private NoParamsEvent playerDeathEvent;
        
        private void Start()
        {
            healthChangedEvent.Raise(Health, maxHealth);
        }

        public override void ApplyDamage(int damage)
        {
            base.ApplyDamage(damage);
            healthChangedEvent.Raise(Health, maxHealth);
            
        }

        protected override void DeathHandler()
        {
            playerDeathEvent.Raise();
        }
    }
}

