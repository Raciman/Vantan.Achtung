using Ach.Units;
using RPool;
using UnityEngine;

namespace Ach.Weapons
{
    public class BulletVfxComponent : MonoBehaviour, IPoolable
    {
        [SerializeField] private TrailRenderer trail;
        public IPool Owner { get; set; }
        public void OnGet()
        {
            trail.Clear();
        }

        public void OnRelease()
        {
            
        }

        public void Despawn()
        {
            Owner.Release(this);
        }
    }
}