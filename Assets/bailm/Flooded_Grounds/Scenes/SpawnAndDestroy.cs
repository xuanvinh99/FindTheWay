using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnAndDestroy : MonoBehaviour
{
 public GameObject endPoint; // Kéo và thả EndPoint vào đây trong Inspector

    void Start()
    {
        // Bắt đầu coroutine để quản lý thời gian
        StartCoroutine(ShowEndPoint());
    }

    private IEnumerator ShowEndPoint()
    {
        // Chờ 1 phút (60 giây)
        yield return new WaitForSeconds(10f);
        
        // Hiện EndPoint
        endPoint.SetActive(true);
        
        // Chờ 30 giây trước khi xóa
        yield return new WaitForSeconds(10f);
        
        // Ẩn EndPoint
        endPoint.SetActive(false);
    }
    

}
