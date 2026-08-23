using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ach
{
    public class BootstrapEntry
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if(currentScene.buildIndex != 0)
                SceneManager.LoadScene(0);
        }
    }
}
