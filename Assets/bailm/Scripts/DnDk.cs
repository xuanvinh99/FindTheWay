using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Hàm này s? ???c gán cho nút
    public void ChangeScene(string TenScene)
    {
        // Chuy?n ??n scene có tên là sceneName
        SceneManager.LoadScene(TenScene);
    }
}