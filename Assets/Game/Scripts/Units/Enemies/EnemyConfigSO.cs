using UnityEngine;

namespace Ach.Units.Enemies
{
    [CreateAssetMenu(menuName = "SO/Enemy/Enemy Config SO")]
    public class EnemyConfigSO : ScriptableObject
    {
        //States
        [field: SerializeField] public float IdleDuration { get; private set; }
        [field: SerializeField] public float WanderRadius { get; private set; }
        [field: SerializeField] public float TargetDetectRadius { get; private set; }
        [field: SerializeField] public float TargetLoseRadius { get; private set; }
        //Attack
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackHitRange { get; private set; }
        [field: SerializeField] public float AttackAngle { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
        [field: SerializeField] public float AttackDuration { get; private set; }
        [field: SerializeField] public float AttackHitTime { get; private set; }
        
        
    }
}