using UnityEngine;

namespace Ach.Weapons
{
    [CreateAssetMenu(menuName = "SO/Weapons/WeaponConfigSO")]
    public class WeaponConfigSO : ScriptableObject
    {

        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public float Spread { get; private set; }
        [field: SerializeField] public BulletVfxComponent BulletVfx { get; private set; }
        [field: SerializeField] public float BulletVfxSpeed { get; private set; }
    }
}

