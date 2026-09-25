using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ach.UI
{
    public sealed class SceneLoader : MonoBehaviour
    {
        [SerializeField, Min(0), Tooltip("Scene ID (build index) from the enabled Build Settings scenes.")]
        private int sceneId;
        private bool _isLoading;
        public int SceneId => sceneId;

        public void LoadScene()
        {
            if (_isLoading)
                return;
            if (sceneId < 0 || sceneId >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError($"Scene ID {sceneId} is not enabled in Build Settings.", this);
                return;
            }
            _isLoading = true;
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(sceneId);
        }
    }
}
