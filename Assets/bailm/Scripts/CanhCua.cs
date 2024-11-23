using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CanhCua : MonoBehaviour
{
    public float thoiGianHien = 30f; // Thời gian capsule xuất hiện
    public float thoiGianAn = 30f;   // Thời gian capsule biến mất
    public string tenSceneDich;      // Tên scene cần chuyển
    public GameObject duongDan;      // Đường chỉ dẫn

    private Renderer rend;
    private Collider col;

    private void Start()
    {
        rend = GetComponent<Renderer>(); // Lấy Renderer của capsule
        col = GetComponent<Collider>();   // Lấy Collider của capsule

        rend.enabled = false; // Bắt đầu với trạng thái ẩn
        col.enabled = false;

        if (duongDan != null)
        {
            duongDan.SetActive(false); // Đường chỉ dẫn bắt đầu với trạng thái ẩn
        }

        StartCoroutine(DoiTrangThaiHienAn());
    }

    private IEnumerator DoiTrangThaiHienAn()
    {
        yield return new WaitForSeconds(thoiGianAn); // Chờ trước khi xuất hiện lần đầu

        while (true)
        {
            rend.enabled = true;   // Hiện capsule
            col.enabled = true;

            if (duongDan != null)
            {
                duongDan.SetActive(true); // Hiện đường chỉ dẫn
            }

            yield return new WaitForSeconds(thoiGianHien);

            rend.enabled = false;  // Ẩn capsule
            col.enabled = false;

            if (duongDan != null)
            {
                duongDan.SetActive(false); // Ẩn đường chỉ dẫn
            }

            yield return new WaitForSeconds(thoiGianAn);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            SceneManager.LoadScene(tenSceneDich); // Chuyển scene khi player chạm vào
        }
    }
}
