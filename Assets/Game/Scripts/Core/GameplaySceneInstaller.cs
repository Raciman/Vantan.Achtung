using Ach.Drop;
using Ach.Sfx;
using Reflex.Core;
using RPool;
using UnityEngine;

namespace Ach
{
    public class GameplaySceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private PoolService poolService;
        [SerializeField] private DropListSO dropList;
        [SerializeField] private AudioService audioService;
        [SerializeField] private QuestManager questManager;
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterValue(poolService);
            var dropService = new DropService(dropList, poolService);
            containerBuilder.RegisterValue(dropService);
            containerBuilder.RegisterValue(audioService);
            containerBuilder.RegisterValue(questManager);
        }
    }
}

