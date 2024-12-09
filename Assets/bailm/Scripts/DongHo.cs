using UnityEngine;
using UnityEngine.UI;

public class DongHo : MonoBehaviour
{
    public Text timerText; 
    private float elapsedTime = 0f; 

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
