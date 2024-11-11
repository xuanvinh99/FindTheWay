using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBangP : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGameFunction();
            }
        }
    }

    void PauseGameFunction()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Hàm tiếp tục trò chơi
    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }
}
