using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // H�m n�y s? ???c g�n cho n�t
    public void ChangeScene(string TenScene)
    {
        // Chuy?n ??n scene c� t�n l� sceneName
        SceneManager.LoadScene(TenScene);
    }
}