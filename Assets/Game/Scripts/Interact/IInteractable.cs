using UnityEngine;

namespace Ach.Interact
{
    public interface IInteractable
    {
        Vector3 Position { get; }
        void Interact(IInteractor interactor);
    }
}

