using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausseMenu;
    public void Pause()
    {
        pausseMenu.SetActive(true);
        Time.timeScale = 0;
    }
   
    public void Exit()
    {
        SceneManager.LoadScene("Manhinhchao");
        Time.timeScale = 1;
    }

    public void Close()
    {
        pausseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
