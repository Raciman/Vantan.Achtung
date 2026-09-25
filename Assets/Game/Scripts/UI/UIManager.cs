using Ach.Event;
using PrimeTween;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Ach.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject interactTip;
        [SerializeField] private BoolEvent interactTipEvent;
        
        [SerializeField] private RectTransform questTip;
        [SerializeField] private LocalizeStringEvent questTipText;
        
        [SerializeField] private float questTipSlideDistance = 250f;
        [SerializeField] private float questTipMoveDuration = 0.4f;
        private float _questTipY;
        private Sequence _questTipSequence;
        
        private void Awake()
        {

            interactTip.SetActive(false);
            interactTipEvent.OnEvent += InteractTipHandler;
            questTip.gameObject.SetActive(false);
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
            
            if (_questTipSequence.isAlive)
                _questTipSequence.Stop();

        }
        
        
    }
}

