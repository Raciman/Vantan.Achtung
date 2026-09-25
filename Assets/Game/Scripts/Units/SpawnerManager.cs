using System;
using Reflex.Attributes;
using RPool;
using UnityEngine;

namespace Ach.Units.Enemies
{
    public sealed class SpawnerManager : MonoBehaviour
    {
        [SerializeField] private HealthComponent player;
        [SerializeField] private EnemyRoot enemyPrefab;
        
        [SerializeField] private SpawnZone[] spawnZones;

        [Inject] private PoolService _pool;


        private void Awake()
        {
            foreach (var zone in spawnZones)
            {
                for (int i = 0; i < zone.EnemiesToSpawn; i++)
                {
                    if(zone.TryGetPoint(out var point))
                    {
                        var instance = _pool.Get(enemyPrefab, point, Quaternion.identity);
                        instance.Init(player);
                    }
                }

            }

        }
    }
}

