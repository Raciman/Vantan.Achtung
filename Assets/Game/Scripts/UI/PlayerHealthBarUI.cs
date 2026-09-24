using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ach.Events
{
    public class PlayerHealthBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private IntIntEvent healthChangedEvent;

        private void Awake()
        {
            healthChangedEvent.OnEvent += HealthChangedHandler;
        }

        private void HealthChangedHandler(int currentHealth, int maxHealth)
        {
            healthText.text = currentHealth + " / " + maxHealth;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        private void OnDestroy()
        {
            healthChangedEvent.OnEvent -= HealthChangedHandler;
        }
    }
}

