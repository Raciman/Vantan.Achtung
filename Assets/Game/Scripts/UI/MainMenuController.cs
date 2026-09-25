using Ach.Input;
using Reflex.Attributes;
using UnityEngine;

namespace Ach.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        [Inject] private IInputService _input;

        private void Awake()
        {
            _input.SetGameplayEnabled(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
