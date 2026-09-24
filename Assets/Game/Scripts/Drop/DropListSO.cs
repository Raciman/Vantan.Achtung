using System.Collections.Generic;
using Ach.Weapons;
using UnityEngine;

namespace Ach.Drop
{
    [CreateAssetMenu(menuName = "SO/Drop/DropList")]
    public class DropListSO : ScriptableObject
    {
        [field : SerializeField] public List<WeaponPickup> Weapons { get; private set; }
        
    }
}

