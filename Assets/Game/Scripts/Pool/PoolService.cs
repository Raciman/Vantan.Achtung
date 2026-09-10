using System;
using System.Collections.Generic;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

namespace RPool
{
    public class PoolService : MonoBehaviour
    {
        private Dictionary<Component, IPool> _pools = new ();

        private Transform _root;
        
        private const int PreloadCount = 1;
        
        public event Action<Component> Spawned;
        public event Action<Component> Released;
        
        [Inject] private Container _container;

        private void Awake() 
        {
            _root = new GameObject("[POOLED]").transform;
            _root.SetParent(transform, false);
        }
        
        public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component, IPoolable
        {
            if (!_pools.TryGetValue(prefab, out var pool))
            {
                pool = new Pool<T>(prefab, _root);
                pool.Created += component => GameObjectInjector.InjectObject(component.gameObject, _container);
                pool.Released += PoolReleasedHandler;
                pool.Prewarm(PreloadCount, _root);
                _pools[prefab]  = pool;
            }
            
            var instance =  (T)pool.Get();
            instance.transform.SetPositionAndRotation(position, rotation);
            Spawned?.Invoke(instance);
            return instance;
        }

        private void PoolReleasedHandler(Component component)
        {
            Released?.Invoke(component);
        }

        public void ReleaseAll(Component poolKeyPrefab)
        {
            if (_pools.TryGetValue(poolKeyPrefab, out var pool))
            {
                pool.ReleaseAll();
            }
        }
    }
}
