using Ach.Event;
using Ach.Events;
using Ach.Input;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Ach.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Pause menu")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private UnityEngine.UI.Button continueButton;
        [SerializeField] private GameObject loseMenu;
        
        [Inject] private IInputService _input;
        private float _previousTimeScale;
        private CursorLockMode _previousCursorLock;
        private bool _previousCursorVisible;
        public bool IsMenuOpen { get; private set; }

        [SerializeField] private GameObject interactTip;
        [SerializeField] private BoolEvent interactTipEvent;
        
        [SerializeField] private RectTransform questTip;
        [SerializeField] private LocalizeStringEvent questTipText;
        
        [SerializeField] private float questTipSlideDistance = 250f;
        [SerializeField] private float questTipMoveDuration = 0.4f;
        private float _questTipY;
        private Sequence _questTipSequence;
        
        [SerializeField] private NoParamsEvent playerDeathEvent;
        
        private void Awake()
        {
            if (pauseMenu != null)
                pauseMenu.SetActive(false);

            interactTip.SetActive(false);
            interactTipEvent.OnEvent += InteractTipHandler;
            questTip.gameObject.SetActive(false);

            playerDeathEvent.OnEvent += PlayerDeathHandler;
        }

        private void PlayerDeathHandler()
        {
            loseMenu.SetActive(true);
        }

        private void OnEnable()
        {
            _input.PausePressed += ToggleMenu;
            _input.SetGameplayEnabled(true);
        }

        public void ToggleMenu()
        {
            if (pauseMenu == null)
                return;
            if (IsMenuOpen)
            {
                CloseMenu();
                return;
            }
            _previousTimeScale = Time.timeScale;
            _previousCursorLock = Cursor.lockState;
            _previousCursorVisible = Cursor.visible;
            IsMenuOpen = true;
            _input.SetGameplayEnabled(false);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            if (continueButton != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }

        public void CloseMenu()
        {
            if (!IsMenuOpen)
                return;
            RestorePauseState();
            _input.SetGameplayEnabled(true);
        }

        private void RestorePauseState()
        {
            IsMenuOpen = false;
            if (pauseMenu != null)
                pauseMenu.SetActive(false);
            Time.timeScale = _previousTimeScale;
            Cursor.lockState = _previousCursorLock;
            Cursor.visible = _previousCursorVisible;
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void OnDisable()
        {
            _input.PausePressed -= ToggleMenu;
            if (IsMenuOpen)
                RestorePauseState();
            _input.SetGameplayEnabled(false);
        }

        private void Start()
        {
            _questTipY = questTip.anchoredPosition.y;
        }

        private void InteractTipHandler(bool active)
        {
            interactTip.SetActive(active);
        }
        
        private void AnimateQuestTip()
        {
            if (questTip == null)
                return;

            if(_questTipSequence.isAlive)
                return;
            
            questTip.gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();


            float hiddenY = _questTipY + questTipSlideDistance;

            questTip.anchoredPosition = new Vector2(
                questTip.anchoredPosition.x, hiddenY);

            _questTipSequence = Sequence.Create(useUnscaledTime: true)
                .Chain(Tween.UIAnchoredPositionY(
                    questTip,
                    startValue: hiddenY,
                    endValue: _questTipY,
                    duration: questTipMoveDuration,
                    ease: Ease.OutCubic))
                .ChainDelay(5f)
                .Chain(Tween.UIAnchoredPositionY(
                    questTip,
                    startValue: _questTipY,
                    endValue: hiddenY,
                    duration: questTipMoveDuration,
                    ease: Ease.InCubic))
                .OnComplete(questTip, tip => tip.gameObject.SetActive(false));
        }
        

        public void ShowQuestTip(LocalizedString text)
        {
            questTipText.StringReference = text;
            AnimateQuestTip();
        }

        private void OnDestroy()
        {
            interactTipEvent.OnEvent -= InteractTipHandler;
            playerDeathEvent.OnEvent -= PlayerDeathHandler;

            if (_questTipSequence.isAlive)
                _questTipSequence.Stop();

        }
        
        
    }
}

