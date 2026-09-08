using System;
using Ach.Weapons;
using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private WeaponController[] equippedWeapons;

        private WeaponController _currentWeapon;
        private float _cooldown;
        public int CurrentSlot { get; private set; }
        public int PendingSlot { get; private set; }
        public bool HasWeapon => CurrentSlot != 0;
        public bool CanFire => _cooldown <= 0f && HasWeapon; 
        public bool CanReload => HasWeapon;  // + && !IsMagazineFull TODO

        private void Awake()
        {
            _currentWeapon =  equippedWeapons[0]; //TODO временно для теста
        }

        public void Tick(float deltaTime)
        {
            if(_cooldown > 0f)
                _cooldown -= deltaTime;
            if(HasWeapon)
                _currentWeapon.Tick(deltaTime);
        }

        public void StartReload()
        {
            
            //TODO _currentWeapon.Reload();
        }
        

        public void Fire()
        {
            _currentWeapon.Shoot();

            //_cooldown = fireRate

        }

        public void CancelReload()
        {
            //TODO
        }

        public void RequestSlot(int index) => PendingSlot = (CurrentSlot == index) ? 0 : index;
        

        public void ApplyPendingSlot() => CurrentSlot = PendingSlot;
    }
}

