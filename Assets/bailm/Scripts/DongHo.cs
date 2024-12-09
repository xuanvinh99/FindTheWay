using UnityEngine;
using UnityEngine.UI;

public class DongHo : MonoBehaviour
{
    public Text timerText;
    private float countdownTime = 120f; 
    private float remainingTime; 

    private void Start()
    {
        remainingTime = countdownTime;
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime; 

            if (remainingTime <= 0)
            {
                remainingTime = 0; 
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }
        else
        {
            ResetCountdown(); 
        }
    }

    private void ResetCountdown()
    {
        remainingTime = countdownTime; 
    }
}
