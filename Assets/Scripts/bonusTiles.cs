using UnityEngine;

public class bonusTiles : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthManagement health = other.GetComponentInParent<HealthManagement>();

            health.Heal(20);
        }
    }
}
