using System;
using UnityEngine;

namespace Ach.Units.Player
{
    [Serializable]
    public class WeaponAnimSet
    {
        [SerializeField] private int layer;              
        [SerializeField] private string idleState    = "Idle";
        [SerializeField] private string aimState     = "Aim";
        [SerializeField] private string drawState    = "Draw";
        [SerializeField] private string holsterState = "Holster";
        [SerializeField] private string fireState    = "Fire";
        [SerializeField] private string reloadState  = "Reload";
        
        [SerializeField] private AnimationClip drawClip, holsterClip, fireClip, reloadClip;
        
        public int Layer => layer;
        public int Idle { get; private set; }
        public int Aim  { get; private set; }
        public int Draw { get; private set; }
        public int Holster { get; private set; }
        public int Fire { get; private set; }
        public int Reload { get; private set; }
        
        public float DrawLength    => drawClip    ? drawClip.length    : 0f;
        public float HolsterLength => holsterClip ? holsterClip.length : 0f;
        public float FireLength    => fireClip    ? fireClip.length    : 0f;
        public float ReloadLength  => reloadClip  ? reloadClip.length  : 0f;
        
        public void BuildHashes(Animator animator)
        {
            string prefix = animator.GetLayerName(layer) + ".";
            Idle    = Animator.StringToHash(prefix + idleState);
            Aim     = Animator.StringToHash(prefix + aimState);
            Draw    = Animator.StringToHash(prefix + drawState);
            Holster = Animator.StringToHash(prefix + holsterState);
            Fire    = Animator.StringToHash(prefix + fireState);
            Reload  = Animator.StringToHash(prefix + reloadState);
        }
    }
}

