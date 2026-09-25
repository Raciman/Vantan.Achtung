using System;
using UnityEngine;

namespace Ach.Units.Enemies
{
    public interface IEnemyIntent
    {
        float DistanceToTarget { get; }
        bool CanSeeTarget { get; }
        bool HasLostTarget { get; }
        Vector3 LastSeenTargetPosition { get; }
        bool WasHit { get; }
        void ConsumeHit();

    }
    
    public sealed class EnemyIntentProvider : IEnemyIntent, IDisposable
    {
        private const float EyeHeight = 1.5f;
        private const float LostSightDelay = 0.3f;
        private const float HitMemory = 0.3f;
        
        private readonly Transform _owner;
        private readonly Transform _target;
        private readonly LayerMask _obstacleMask;

        private float _lastSeenTime;
        private float _hitAt = float.NegativeInfinity;
        private HealthComponent _health;
        
        public float DistanceToTarget { get; private set; }
        public bool CanSeeTarget { get; private set; }
        public bool HasLostTarget { get; private set; }
        public Vector3 LastSeenTargetPosition { get; private set; }
        public bool WasHit => Time.time - _hitAt <= HitMemory;

        public EnemyIntentProvider(Transform owner, Transform target, HealthComponent health,
            LayerMask obstacleMask)
        {
            _owner = owner;
            _target = target;
            _obstacleMask = obstacleMask;
            _health = health;
            _health.Damaged += DamagedHandler;
        }

        private void DamagedHandler()
        {
            _hitAt = Time.time;
            _lastSeenTime = Time.time;
            LastSeenTargetPosition = _target.position;
        }

        public void Tick(float deltaTime)
        {
            DistanceToTarget = Vector3.Distance(_owner.position, _target.position);
            
            Vector3 eye = _owner.position + Vector3.up * EyeHeight;
            Vector3 target = _target.position + Vector3.up * EyeHeight;
            
            Vector3 dir =  target - eye;
            CanSeeTarget = !Physics.Raycast(eye, dir.normalized, 
                dir.magnitude, _obstacleMask);
            if (CanSeeTarget)
            {
                LastSeenTargetPosition = _target.position;
                _lastSeenTime = Time.time;
            }
            HasLostTarget = Time.time - _lastSeenTime > LostSightDelay;
        }

        public void ConsumeHit() => _hitAt = float.NegativeInfinity;
        


        public void Dispose()
        {
            _health.Damaged -= DamagedHandler;
        }
    }
}