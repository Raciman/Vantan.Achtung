using System;
using Ach.Interact;
using RPool;
using UnityEngine;

namespace Ach.Weapons
{
    public sealed class WeaponPickup : MonoBehaviour, IInteractable, IPoolable
    {
        [field : SerializeField] public WeaponConfigSO Weapon { get; private set; }

        private DropWeaponInfo _info;

        private IInteractor _register;

        private void Awake()
        {
            _info = new DropWeaponInfo(Weapon.MaxBulletCapacity, Weapon.BulletShotCount);
        }

        public void SetDropInfo(DropWeaponInfo info)
        {
            _info = info;
        }

        public Vector3 Position => transform.position;
        public void Interact(IInteractor interactor)
        {
            if (interactor.TryTakeWeapon(Weapon, _info))
                Despawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent(out IInteractor player))
                return;
            
            _register = player;
            player.AddInteractable(this);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!other.TryGetComponent(out IInteractor player))
                return;
            
            _register = null;
            player.RemoveInteractable(this);
        }

        public IPool Owner { get; set; }
        public void OnGet()
        {
            
        }

        public void OnRelease()
        {
            _register?.RemoveInteractable(this);
            _register = null;
        }

        private void Despawn()
        {
            if(Owner != null) Owner.Release(this);
            else
            {
                OnRelease();
                Destroy(gameObject);
            }
        }
    }
}

