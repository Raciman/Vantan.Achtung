using UnityEngine;

namespace Ach.Units
{
    public interface IDamageable
    {
        Transform Transform { get; }
        void ApplyDamage(int damage);
    }
    
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] protected int maxHealth = 100;

        protected int Health;
        public Transform Transform => gameObject.transform;
        public bool IsDead => Health <= 0;

        private void Awake()
        {
            Health = maxHealth;
        }
        
        public virtual void ApplyDamage(int damage)
        {
            if(damage > Health)
                damage = Health;
            
            Health -= damage;

            Debug.Log(name + " current HP " + Health);
            if (Health <= 0)
                DeathHandler();
        }

        private void DeathHandler()
        {
            Debug.Log(name + "Death");
        }
    }
    

}

