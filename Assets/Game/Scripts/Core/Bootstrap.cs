using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ach
{
    public class Bootstrap : MonoBehaviour
    {
     private static Bootstrap _instance;
            
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
                
                IsInitialized = true;
                
                SceneManager.LoadScene("MenuScene");
            }
    }
}

