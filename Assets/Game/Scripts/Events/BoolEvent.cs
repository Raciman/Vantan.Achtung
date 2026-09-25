using System;
using UnityEngine;

namespace Ach.Events
{
    [CreateAssetMenu(menuName = "SO/Events/BoolEvent")]
    public class BoolEvent : ScriptableObject
    {
        public event Action<bool> OnEvent;
        public void Raise(bool value) => OnEvent?.Invoke(value);
    }
}

