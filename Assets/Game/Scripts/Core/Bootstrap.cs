using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ach
{
    public class Bootstrap : MonoBehaviour
    {
     private static Bootstrap _instance;
          //  [Inject] private SaveService _saveService;
            
            public static bool IsInitialized { get; private set; }
    
            private void Awake()
            {
                if (_instance != null && _instance != this)
                {
                    Destroy(gameObject);
                    return;
                }
    
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
    
            private async void Start()
            {
                if(IsInitialized)
                    return;
    
                /*try
                {
                    await _saveService.LoadAsync();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Save load failed: {e}");
                }*/
                
                IsInitialized = true;
                //SetLanguage();
                
                SceneManager.LoadScene(1);
            }
    }
}

