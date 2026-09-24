using Ach.Sfx;
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

        [field:SerializeField] public Transform LeftHandTargetIk { get; private set; }

        private float _laserLength = 3f;
        private bool _laserActive;

        [Inject] private PoolService _pool;
        [Inject] private AudioService _audioService;


        public int Ammo { get; private set; }
        public int AmmoInClip  { get; private set; }
        public bool IsMagazineFull => AmmoInClip >= config.ClipCapacity;
        public int AnimLayer => config.AnimLayer;
        public WeaponConfigSO Config => config;
        public Transform ShootPoint => shootPoint;

        public void InitDrop(DropWeaponInfo dropInfo)
        {
            Ammo = dropInfo.Ammo;
            AmmoInClip = dropInfo.AmmoInClip;
        }
        
        private void Awake()
        {
            laser.enabled = false;
            
            //TODO Перенести в инит мб
            Ammo = config.MaxBulletCapacity;
            AmmoInClip = config.ClipCapacity;
        }

        public void Tick(float deltaTime)
        {
            if(_laserActive)
                UpdateAimLaser();
        }

        public void Shoot()
        {
            if(AmmoInClip <= 0) return;
            
            float half = config.Spread * 0.5f;
            for (int i = 0; i < config.BulletShotCount; i++)
            {
                float angle = Random.Range(-half, half);
                ShootRay(Quaternion.AngleAxis(angle, Vector3.up) * shootPoint.forward);
            }

            _audioService.PlayAt(Config.SfxType, transform.position);
            AmmoInClip--;
        }

        public void CompleteReload()
        {
            int take = Mathf.Min(config.ClipCapacity - AmmoInClip, Ammo);
            AmmoInClip += take;
            Ammo -= take;
        }
        
        
        private void ShootRay(Vector3 direction)
        {
            var trail = _pool.Get(config.BulletVfx, shootPoint.position, shootPoint.rotation);
            if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit,
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
                Vector3 hitPoint = shootPoint.position + direction * config.Range;
                float travelTime = Vector3.Distance(shootPoint.position, hitPoint) / config.BulletVfxSpeed;
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
                    _laserLength + 1f, mask, QueryTriggerInteraction.Ignore))
            {
                endPoint = hit.point;
                tipLength = 0f;
            }
            laser.SetPosition(1, endPoint);
            laser.SetPosition(2, endPoint + shootPoint.forward * tipLength);
        }

        public void SetLaserActive(bool active)
        {
            if(_laserActive == active)
                return;
            _laserActive = active;
            laser.enabled = active;
        }


    }
}

