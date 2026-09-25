using Ach.Interact;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Ach.Quest
{
    public class QuestZone : MonoBehaviour, IInteractable
    {
        [SerializeField] private Collider collider;
        [SerializeField] private GameObject vfx;
        [SerializeField] private GameObject questObject;
        [field : SerializeField] public LocalizedString QuestTip {get; private set;}
        
        private IInteractor _register;

        
        
        [Inject] private QuestManager _questManager;
        
        public Vector3 Position => transform.position;

        public void Activate(bool active)
        {
            collider.enabled = active;
            
            if(vfx == null) return;
            vfx.SetActive(active);

        }


        public void Interact(IInteractor interactor)
        {
            _questManager.ProgressStep();
            questObject.SetActive(false);
            
            _register.RemoveInteractable(this);
            _register = null;
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
    }
}

