using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int current, int max)
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {current} / {max}";
        }
    }
}