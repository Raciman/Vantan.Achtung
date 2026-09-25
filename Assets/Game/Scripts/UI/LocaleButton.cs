using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Ach.UI
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public sealed class LocaleButton : MonoBehaviour
    {
        [SerializeField] private string localeCode = "en";
        [SerializeField] private GameObject selectedIndicator;

        private void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += UpdateSelection;
            StartCoroutine(InitializeSelection());
        }

        private IEnumerator InitializeSelection()
        {
            yield return LocalizationSettings.InitializationOperation;
            UpdateSelection(LocalizationSettings.SelectedLocale);
        }

        public void SelectLocale() => StartCoroutine(SelectWhenReady());

        private IEnumerator SelectWhenReady()
        {
            yield return LocalizationSettings.InitializationOperation;
            var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
            if (locale == null)
                yield break;
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString("selected-locale", locale.Identifier.Code);
            PlayerPrefs.Save();
        }

        private void UpdateSelection(Locale locale)
        {
            if (selectedIndicator != null)
                selectedIndicator.SetActive(locale != null && locale.Identifier.Code == localeCode);
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= UpdateSelection;
            StopAllCoroutines();
        }
    }
}
