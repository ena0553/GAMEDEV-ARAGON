
using UnityEngine;
using UnityEngine.UI;

public class HealthManagement : MonoBehaviour
{

    public Image HealthBar;
    public int maxHealth = 100;
    public int healthAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthAmount = maxHealth;

        HealthBar.fillAmount = (int)healthAmount / 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthAmount <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("[LR] Main");
        }

    }   

    public void TakeDamage(int damage)
    {
        healthAmount -= 20;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth);

        HealthBar.fillAmount = (float)healthAmount / 100;
    }

    public void Heal(int healAmount)
    {
        healthAmount += 20;
        healthAmount = Mathf.Clamp(healthAmount , 0, 100);

        HealthBar.fillAmount = (float)healthAmount / 100;
    }

}
