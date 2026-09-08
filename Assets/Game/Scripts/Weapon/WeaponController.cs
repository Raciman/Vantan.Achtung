using Ach.Units;
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
        
        public Transform ShootPoint => shootPoint;

        public void Tick(float deltaTime)
        {
            UpdateAimLaser();
        }
        
        public void Shoot()
        {

            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit,
                    config.Range, mask, QueryTriggerInteraction.Ignore))
            {
                hit.collider.GetComponentInParent<IDamageable>()?.ApplyDamage(config.Damage);
            }
            //spawn vfx
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

