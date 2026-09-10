using Ach.Units;
using PrimeTween;
using Reflex.Attributes;
using RPool;
using UnityEngine;

namespace Ach.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private WeaponConfigSO config;
        [SerializeField] private LayerMask mask;
        
        [SerializeField] private LineRenderer laser;

        private float _laserLength = 3f;

        [Inject] private PoolService _pool;
        
        public Transform ShootPoint => shootPoint;

        public void Tick(float deltaTime)
        {
            UpdateAimLaser();
        }
        
        public void Shoot()
        {
            var trail = _pool.Get(config.BulletVfx, shootPoint.position, shootPoint.rotation);
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit,
                    config.Range, mask, QueryTriggerInteraction.Ignore))
            {
                float travelTime = Vector3.Distance(shootPoint.position, hit.point) / config.BulletVfxSpeed;
                var damageable = hit.collider.GetComponentInParent<IDamageable>() as Component;

                Tween.Position(trail.transform, hit.point, travelTime).OnComplete(() =>
                {
                    if(damageable)
                    ((IDamageable)damageable).ApplyDamage(config.Damage);
                    trail.Despawn();
                });

            }
            else
            {
                float travelTime = Vector3.Distance(shootPoint.position, shootPoint.position * 3f) / config.BulletVfxSpeed;
                Vector3 hitPoint = shootPoint.position + shootPoint.forward * config.Range;
                Tween.Position(trail.transform, hitPoint, travelTime).OnComplete(() =>
                {
                    trail.Despawn();
                });
            }
        }
        
        private void UpdateAimLaser()
        {
            float tipLength = 0.5f;
            laser.SetPosition(0, shootPoint.position);
            Vector3 endPoint = shootPoint.position + shootPoint.forward * _laserLength;
            if (Physics.Raycast(shootPoint.position - shootPoint.forward, shootPoint.forward, out RaycastHit hit,
                    _laserLength + 1f))
            {
                endPoint = hit.point;
                tipLength = 0f;
            }
            laser.SetPosition(1, endPoint);
            laser.SetPosition(2, endPoint + shootPoint.forward * tipLength);
        }
    }
}

