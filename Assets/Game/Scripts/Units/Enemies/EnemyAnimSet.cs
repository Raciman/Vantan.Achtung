using System;
using UnityEngine;

namespace Ach.Units.Enemies
{
    [Serializable]
    public class EnemyAnimSet
    {
        [SerializeField] private AnimationClip clip;
        
        public float Length => clip ? clip.length : 0f;


    }
}

