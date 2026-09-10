using Ach.Input;
using Reflex.Core;
using RPool;
using UnityEngine;

public class RootInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        IInputService inputService = new InputService();
        containerBuilder.RegisterValue(inputService, new[] { typeof(IInputService) });
    }
}
