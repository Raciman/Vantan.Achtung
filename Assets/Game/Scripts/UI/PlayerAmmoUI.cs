using Ach.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ach.UI
{
    public class PlayerAmmoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private Image ammoIcon;
        [SerializeField] private IntIntEvent ammoChangedEvent;
        [SerializeField] private BoolEvent holsterWeaponEvent;

        private void Awake()
        {
            SwitchVisual(false);
            
            ammoChangedEvent.OnEvent += UpdateAmmoText;
            holsterWeaponEvent.OnEvent += SwitchVisual;
        }

        private void SwitchVisual(bool isActive)
        {
            ammoText.gameObject.SetActive(isActive);
            ammoIcon.gameObject.SetActive(isActive);
            
        }

        private void UpdateAmmoText(int clipAmmo, int storedAmmo)
        {
            ammoText.text = $"{clipAmmo} / {storedAmmo}";
        }

        private void OnDestroy()
        {
            ammoChangedEvent.OnEvent -= UpdateAmmoText;
            holsterWeaponEvent.OnEvent -= SwitchVisual;

        }
    }
}

