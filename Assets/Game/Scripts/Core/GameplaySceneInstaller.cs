using Reflex.Core;
using RPool;
using UnityEngine;

namespace Ach
{
    public class GameplaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private PoolService poolService;
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterValue(poolService);
        }
    }
}

