using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float time = 15;
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;

            if (time < 0)
            {
                time = 0; 
                timerText.text = "Time's up!";
            }
            else
            {
                timerText.text = "Time left: " + Mathf.Round(time).ToString();
            }

        }

    }
}
