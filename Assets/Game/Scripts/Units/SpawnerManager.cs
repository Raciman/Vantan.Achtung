using System;
using Reflex.Attributes;
using RPool;
using UnityEngine;

namespace Ach.Units.Enemies
{
    public class SpawnerManager : MonoBehaviour
    {
        [SerializeField] private HealthComponent player;
        [SerializeField] private EnemyRoot enemyPrefab;
        
        [SerializeField] private Transform spawnPoint;

        [Inject] private PoolService _pool;


        private void Awake()
        {
            for (int i = 0; i < 10; i++)
            {
                var instance = _pool.Get(enemyPrefab,  spawnPoint.position, Quaternion.identity);
                instance.Init(player);

            }
        }
    }
}

