using System.Collections.Generic;
using System.Linq;
using Ach.Drop;
using Ach.Events;
using Ach.Weapons;
using Reflex.Attributes;
using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private List<WeaponController> allWeapons;
        private WeaponController[] _equippedWeapons = new WeaponController [2];
        [SerializeField] private Transform leftHandTargetIk;

        [SerializeField] private IntIntEvent ammoChangedEvent;
        [SerializeField] private BoolEvent holsterWeaponEvent;
        
        private IStanceView _stance;
        private float _cooldown;
        
        [Inject] private DropService _dropService;
        
        public WeaponController CurrentWeapon { get; private set; }
        public WeaponController PendingWeapon { get; private set; }
        public bool HasWeapon => CurrentWeapon != null;
        public bool HasPending => PendingWeapon != null;
        public bool CanFire => HasWeapon && CurrentWeapon.AmmoInClip > 0;
        public bool CanReload => HasWeapon && !CurrentWeapon.IsMagazineFull && CurrentWeapon.Ammo > 0;
        public bool CanSelect(int index) => index >= 0 && index < _equippedWeapons.Length 
                                                       && _equippedWeapons[index] != null;

        public bool IsAutoMode => CurrentWeapon.Config.Mode == FireMode.Auto;

        public float FireInterval => CurrentWeapon.Config.FireRate;

        public float ReloadDuration => CurrentWeapon.Config.ReloadDuration;
        
        public void Init(IStanceView stance)
        {
            _stance = stance;
        }
        
        private void Awake()
        {
            foreach (var weapon in allWeapons)
                if(weapon) weapon.gameObject.SetActive(false);
        }

        public void Tick(float deltaTime)
        {
            if (HasWeapon)
            {
                CurrentWeapon.SetLaserActive(_stance.IsWeaponRaised);
                CurrentWeapon.Tick(deltaTime);
            }
            
        }

        public void StartReload()
        {
            
            //TODO _currentWeapon.Reload();
        }

        public void CompleteReload()
        {
            CurrentWeapon.CompleteReload();
            ammoChangedEvent.Raise(CurrentWeapon.AmmoInClip, CurrentWeapon.Ammo);
        }


        public void Fire()
        {
            CurrentWeapon.Shoot();
            ammoChangedEvent.Raise(CurrentWeapon.AmmoInClip, CurrentWeapon.Ammo);
            _cooldown = CurrentWeapon.Config.FireRate;

        }

        public void CancelReload()
        {
            //TODO
        }

        public void RequestSlot(int index)
        {
            var requested =  _equippedWeapons[index];
            PendingWeapon = CurrentWeapon == requested ? null : requested;

        }


        public void ApplyPendingWeapon()
        {
            if (CurrentWeapon) CurrentWeapon.gameObject.SetActive(false);
            CurrentWeapon = PendingWeapon;
            PendingWeapon = null;
            if (CurrentWeapon)
            {
                CurrentWeapon.gameObject.SetActive(true);
                ammoChangedEvent.Raise(CurrentWeapon.AmmoInClip, CurrentWeapon.Ammo);
            }
            AttachLeftHand();
            holsterWeaponEvent.Raise(CurrentWeapon != null);
        }

        public bool TryEquipWeapon(WeaponConfigSO config, DropWeaponInfo dropInfo)
        {
            if(_equippedWeapons[0] != null && _equippedWeapons[1] != null)
                return false;
            
            if(_equippedWeapons.Any(x => x!= null && x.Config == config))
                return false;

            for (int i = 0; i < _equippedWeapons.Length; i++)
            {
                if (_equippedWeapons[i] == null)
                {
                    _equippedWeapons[i] = allWeapons.FirstOrDefault(x => x.Config == config);
                    _equippedWeapons[i].InitDrop(dropInfo);
                    break;
                }
            }

            return true;

        }

        public void DropWeapon()
        {
            if(CurrentWeapon == null) return;
            
            _dropService.DropWeapon(CurrentWeapon.Config, 
                new DropWeaponInfo(CurrentWeapon.Ammo, CurrentWeapon.AmmoInClip),
                transform.position);
            
            for(int i = 0; i < _equippedWeapons.Length; i++)
                if (_equippedWeapons[i] != null && _equippedWeapons[i].Config == CurrentWeapon.Config)
                    _equippedWeapons[i] = null;
            
            CurrentWeapon.gameObject.SetActive(false);
            CurrentWeapon = null;
            holsterWeaponEvent.Raise(false);
        }

        private void AttachLeftHand()
        {
            if(!CurrentWeapon) return;
            leftHandTargetIk.position = CurrentWeapon.LeftHandTargetIk.position;
            leftHandTargetIk.rotation = CurrentWeapon.LeftHandTargetIk.rotation;
        }
    }
    
}

