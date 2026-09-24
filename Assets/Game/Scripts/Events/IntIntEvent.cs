using System;
using UnityEngine;

namespace Ach.Events
{
    [CreateAssetMenu(menuName = "SO/Events/IntInt")]
    public class IntIntEvent : ScriptableObject
    {
        public event Action<int, int> OnEvent;
        public void Raise(int value1, int value2) => OnEvent?.Invoke(value1, value2);
    }
}

