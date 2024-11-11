using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangMan2 : MonoBehaviour
{
    public string sceneName;
    public float delayTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            StartCoroutine(ChangeSceneAfterDelay());
        }
    }

    private IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene(sceneName);
    }
}
