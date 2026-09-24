using Ach.Weapons;

namespace Ach.Interact
{
    public interface IInteractor
    {
        void AddInteractable(IInteractable interactable);
        void RemoveInteractable(IInteractable interactable);
        bool TryTakeWeapon(WeaponConfigSO weapon, DropWeaponInfo dropInfo);
    }
}