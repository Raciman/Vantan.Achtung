using Ach.Sfx;
using UnityEngine;

namespace Ach.Weapons
{

    public enum FireMode
    {
        Single,
        Auto
    }
    
    [CreateAssetMenu(menuName = "SO/Weapons/WeaponConfigSO")]
    public class WeaponConfigSO : ScriptableObject
    {

        [field: SerializeField] public FireMode Mode { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public float ReloadDuration { get; private set; }
        [field: SerializeField] public float Spread { get; private set; }
        [field: SerializeField] public float FireRate { get; private set; }
        [field: SerializeField] public int BulletShotCount { get; private set; }
        [field: SerializeField] public int MaxBulletCapacity { get; private set; }
        [field: SerializeField] public int ClipCapacity { get; private set; }
        [field: SerializeField] public BulletVfxComponent BulletVfx { get; private set; }
        [field: SerializeField] public float BulletVfxSpeed { get; private set; }
        [field: SerializeField] public int AnimLayer { get; private set; }
        [field: SerializeField] public SoundType SfxType { get; private set; }
    }
}

