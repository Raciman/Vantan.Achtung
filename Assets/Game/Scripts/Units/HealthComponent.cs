using UnityEngine;

namespace Ach.Units
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100;

        private float _health;

        private void Awake()
        {
            _health = maxHealth;
        }

        public void ApplyDamage(float damage)
        {
            if(damage > _health)
                damage = _health;
            
            _health -= damage;

            Debug.Log(name + " current HP " + _health);
            if (_health <= 0)
                DeathHandler();
        }

        private void DeathHandler()
        {
            Debug.Log(name + "Death");
        }
    }
    
    public interface IDamageable
    {
        void ApplyDamage(float damage);
    }
}

