using System;
using System.Diagnostics;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;


    [Header("Damage Feedback")]
    [SerializeField] private float damageCooldown = 0.5f;
    private float lastDamageTime = -999f;

    [Header("Debug")]
    [SerializeField] private bool logResets = true;

    public event Action<int, int> OnHealthChanged;
    public event Action OnPlayerDied;

    private Rigidbody body;
    private bool isResetting;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        if (isResetting) return;
        isResetting = true;

        OnPlayerDied?.Invoke();
        UnityEngine.SceneManagement.SceneManager.LoadScene("RaycastScene");
        

        isResetting = false;
    }
}