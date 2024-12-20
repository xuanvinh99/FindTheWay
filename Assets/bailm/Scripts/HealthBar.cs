using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider healthbarSlider;
    public GameObject gameOverMenu; 

    private float currentHealth;

    

    public void GiveFullHealth(float health)
    {
        healthbarSlider.maxValue = health;
        healthbarSlider.value = health;
        currentHealth = health;
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        healthbarSlider.value = health;

        if (currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;

            AudioSource menuAudio = gameOverMenu.GetComponent<AudioSource>();
            if (menuAudio != null)
            {
                menuAudio.Play();
            }
        }
    }

    // Hàm để chơi lại
    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menuu");
    }

}