using UnityEngine;

namespace Ach.Units
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] private Collider aliveCollider;
        private Rigidbody[] _bodies;
        private Collider[] _colliders;

        private void Awake()
        {
            _bodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
            SetRagdoll(false);
        }

        public void SetRagdoll(bool active)
        {
            aliveCollider.enabled = !active;
            foreach (var rb in _bodies) rb.isKinematic = !active;
            foreach (var c in _colliders) c.enabled = active;

        }
    }
}

