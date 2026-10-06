using UnityEngine;

public class WinCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { 
            Debug.Log("You beat the level!");
        }
    }   
}
