using System;
using UnityEngine;

namespace Ach.Events
{
    [CreateAssetMenu(menuName = "SO/Events/NoParamsEvent")]

    public sealed class NoParamsEvent : ScriptableObject
    {
        public event Action OnEvent;
        public void Raise() => OnEvent?.Invoke();
    }
}

