using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class navigationscene : MonoBehaviour
{
    public string scenename;
    // Start is called before the first frame update
        void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player không
        if (other.CompareTag("Player2"))
        {
            // Chuyển sang Scene 2
             Debug.Log("Player has entered the trigger!");
            SceneManager.LoadScene(scenename); // Thay "Scene2" bằng tên Scene bạn muốn chuyển
        }
    }
}
