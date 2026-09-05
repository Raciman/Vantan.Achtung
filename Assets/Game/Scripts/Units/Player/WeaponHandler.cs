using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class WeaponHandler : MonoBehaviour
    {
        private float _cooldown;
        public int CurrentSlot { get; private set; }
        public int PendingSlot { get; private set; }
        public bool HasWeapon => CurrentSlot != 0;
        public bool CanFire => _cooldown <= 0f; 
        public bool CanReload => HasWeapon;  // + && !IsMagazineFull TODO

        public void Tick(float deltaTime)
        {
            if(_cooldown > 0f)
                _cooldown -= deltaTime;
        }

        public void StartReload()
        {
            
            //TODO _currentWeapon.Reload();
        }
        

        public void Fire()
        {
            //TODO _currentWeapon.Fire()
            
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

