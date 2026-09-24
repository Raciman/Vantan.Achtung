using System.Collections.Generic;
using Ach.Event;
using Ach.Interact;
using Ach.Weapons;
using UnityEngine;

namespace Ach.Units.Player
{
    public sealed class InteractController : MonoBehaviour, IInteractor
    {
        [SerializeField] private WeaponHandler weapon;

        [SerializeField] private BoolEvent interactTipEvent;
        
        private List<IInteractable> _inRange = new ();

        
        public IInteractable Closest
        {
            get
            {
                IInteractable best = null;
                float closest = float.MaxValue;
                foreach (var obj in _inRange)
                {
                    float dist = (obj.Position - transform.position).sqrMagnitude;
                    if (dist < closest)
                    {
                        best = obj;
                        closest = dist;
                    }
                }

                return best;
            }
        }

        public void AddInteractable(IInteractable interactable)
        {
            _inRange.Add(interactable);
            interactTipEvent.Raise(_inRange.Count > 0);
        }

        public void RemoveInteractable(IInteractable interactable)
        {
            _inRange.Remove(interactable);
            interactTipEvent.Raise(_inRange.Count > 0);

        }


        public bool TryTakeWeapon(WeaponConfigSO config, DropWeaponInfo dropInfo)
        {
            return weapon.TryEquipWeapon(config, dropInfo);
        }
    }
}

