using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Replay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
