using System;
using Ach.Event;
using UnityEngine;

namespace Ach.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject interactTip;
        [SerializeField] private BoolEvent interactTipEvent;

        private void Awake()
        {
            interactTip.SetActive(false);
            interactTipEvent.OnEvent += InteractTipHandler;
        }

        private void InteractTipHandler(bool active)
        {
            interactTip.SetActive(active);
        }

        private void OnDestroy()
        {
            interactTipEvent.OnEvent -= InteractTipHandler;

        }
    }
}

