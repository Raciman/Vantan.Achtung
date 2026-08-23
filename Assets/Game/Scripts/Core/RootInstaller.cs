using Ach.Input;
using Reflex.Core;
using UnityEngine;

public class RootInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        var inputService = new InputService();
        containerBuilder.RegisterValue(inputService);
        
        
    }
}
