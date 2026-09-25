using System;
using UnityEngine;

namespace Ach.Units
{
    public interface IDamageable
    {
        Transform Transform { get; }
        void ApplyDamage(int damage);
        bool IsDead { get; }
    }
    
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int maxHealth = 100;

        public event Action Damaged;
        protected int Health;
        public Transform Transform => gameObject.transform;
        public bool IsDead => Health <= 0;

        private void Awake()
        {
            Health = maxHealth;
        }
        
        public virtual void ApplyDamage(int damage)
        {
            if(IsDead)  return;
            
            damage = Mathf.Min(damage, Health);
            Health -= damage;
            Damaged?.Invoke();
            if (IsDead)
                DeathHandler();
        }

        protected virtual void DeathHandler()
        {
            
        }
    }
    

}

