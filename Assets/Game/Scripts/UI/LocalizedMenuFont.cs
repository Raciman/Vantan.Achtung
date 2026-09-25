using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Ach.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class LocalizedMenuFont : MonoBehaviour
    {
        [SerializeField] private LocalizedTmpFont font = new LocalizedTmpFont
        {
            TableReference = "MenuAssets",
            TableEntryReference = "Menu.Font"
        };
        private TMP_Text _text;
        private void Awake() => _text = GetComponent<TMP_Text>();
        private void OnEnable() => font.AssetChanged += ApplyFont;
        private void OnDisable() => font.AssetChanged -= ApplyFont;
        private void ApplyFont(TMP_FontAsset value)
        {
            if (value != null)
                _text.font = value;
        }
    }
}
