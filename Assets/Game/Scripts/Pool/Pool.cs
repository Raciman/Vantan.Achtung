using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RPool
{
    public interface IPoolable
    {
        IPool Owner { get; set; }
        void OnGet();
        void OnRelease();
    }

    public interface IPool 
    {
        event Action<Component> Created;
        event Action<Component> Released;

        Component Get();
        void Release(Component instance);
        void ReleaseAll();
        void Prewarm(int  preloadCount = 0, Transform parent = null);
    }
    
    
    public sealed class Pool<T> : IPool where T : Component, IPoolable
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Stack<T> _available;
        private readonly HashSet<T> _active;

        private readonly List<T> _releaseBuffer = new();

        public event Action<Component> Created;
        public event Action<Component> Released;

        public Pool(T prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _available = new Stack<T>();
            _active = new HashSet<T>();


        }

        public void Prewarm(int preloadCount = 0, Transform parent = null)
        {
            for (int i = 0; i < preloadCount; i++)
            {
                T instance = CreateInstance();
                instance.gameObject.SetActive(false);
                _available.Push(instance);
            }
        }

        Component IPool.Get() => Get();

        public T Get()
        {
            T instance;
            if (_available.Count > 0) instance = _available.Pop();
            else instance = CreateInstance();
            
            _active.Add(instance);
            instance.OnGet();
            instance.gameObject.SetActive(true);
            return instance;
        }

        void IPool.Release(Component instance)
        {
            if(instance is T typed)
                Release(typed);
            else
                Debug.LogError($"[Pool] {instance.GetType().Name} is not a valid instance of {typeof(T)}");
        }

        public void Release(T instance)
        {
            if(!_active.Remove(instance))
                return;
            
            instance.gameObject.SetActive(false);
            instance.OnRelease();
            _available.Push(instance);
            Released?.Invoke(instance);
        }

        void IPool.ReleaseAll() => ReleaseAll();
        public void ReleaseAll()
        {
            if(_active.Count == 0)
                return;
            
            _releaseBuffer.AddRange(_active);
            foreach (var instance in _releaseBuffer)
                Release(instance);
            
            _releaseBuffer.Clear();
        }

        private T CreateInstance()
        {
            T instance =  Object.Instantiate(_prefab, _parent);
            instance.Owner = this;
            Created?.Invoke(instance);
            return instance;
        }
    }
}

