using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NutPlay : MonoBehaviour
{
   
    public void ChangeScene()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
