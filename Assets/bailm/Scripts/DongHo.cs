using UnityEngine;
using UnityEngine.UI;

public class DongHo : MonoBehaviour
{
    public Text timerText;           // Hiển thị thời gian
    public AudioSource tickSound;    // Âm thanh tích tắc

    private float countdownTime = 120f; // Tổng thời gian đếm ngược
    private float remainingTime;       // Thời gian còn lại
    private int lastDisplayedSecond;   // Giây cuối cùng được hiển thị

    private void Start()
    {
        remainingTime = countdownTime;
        lastDisplayedSecond = Mathf.FloorToInt(remainingTime);
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

            if (seconds != lastDisplayedSecond)
            {
                lastDisplayedSecond = seconds;
                PlayTickSound();
            }
        }
        else
        {
            ResetCountdown();
        }
    }

    private void PlayTickSound()
    {
        if (tickSound != null)
        {
            tickSound.Play();
        }
        else
        {
            Debug.LogWarning("Chưa gán AudioSource cho tickSound!");
        }
    }

    private void ResetCountdown()
    {
        remainingTime = countdownTime;
    }
}
