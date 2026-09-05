using UnityEngine;

namespace Ach.Units.Player
{
    [CreateAssetMenu(menuName = "SO/Player/PlayerConfig")]
    public sealed class PlayerConfigSO : ScriptableObject
    {
        [field: SerializeField] public float MaxHealth { get; private set; }
        
        [field: SerializeField] public float Gravity { get; private set; }
        [field: SerializeField] public float Speed  { get; private set; }
        [field: SerializeField] public float SprintSpeedMultiplier { get; private set; }
        [field: SerializeField] public float ReloadSpeedMultiplier { get; private set; }

        [field: SerializeField] public float Acceleration { get; private set; }
        [field: SerializeField] public float Deceleration { get; private set; }
        [field: SerializeField] public float ImpulseDamping { get; private set; }
        [field: SerializeField] public float TurnRate { get; private set; }
        [field: SerializeField] public float RotateSpeed { get; private set; }
        
        [field: SerializeField] public float HolsterDuration { get; private set; }
        [field: SerializeField] public float DrawDuration { get; private set; }
        [field: SerializeField] public float FireRecovery { get; private set; }
        [field: SerializeField] public float ReloadRecovery { get; private set; }
    }
}

