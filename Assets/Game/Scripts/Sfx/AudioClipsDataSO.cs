using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ach.Sfx
{
    public enum SoundType
    {
        PistolShot,
        ShotgunShot,
        AutoRifleShot,
        BGM,
    }
    
    [Serializable]
    public struct AudioData
    {
        public SoundType type;
        public AudioClip audioClip;
    }
    
    [CreateAssetMenu(menuName = "SO/Audio/Audio Clips Data")]
    public class AudioClipsDataSO : ScriptableObject
    {
        public List<AudioData> audioClipsData;

        public AudioClip GetClip(SoundType type) => audioClipsData.Find(a => a.type == type).audioClip;
    }
}

