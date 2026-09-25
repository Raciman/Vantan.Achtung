using UnityEngine;
using UnityEngine.AI;

namespace Ach.Units.Enemies
{
    public sealed class SpawnZone : MonoBehaviour
    {
        [field : SerializeField] public int EnemiesToSpawn { get; private set; }
        [SerializeField] private float navMeshSnapDistance = 0.5f;
        [SerializeField] private int maxAttempts = 10;
        
        public bool TryGetPoint(out Vector3 point)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                var local = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
                var world = transform.TransformPoint(local);

                if (NavMesh.SamplePosition(world, out var hit, navMeshSnapDistance, NavMesh.AllAreas))
                {
                    point = hit.position;
                    return true;
                }
            }
            point = default;
            return false;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}

