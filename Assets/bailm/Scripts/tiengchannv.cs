using UnityEngine;

public class tiengchannv : MonoBehaviour
{
    public AudioSource audioSource; // Nguồn phát âm thanh
    public AudioClip footstepSound; // Tệp âm thanh bước chân
    public float stepInterval = 0.5f; // Khoảng thời gian giữa các bước chân

    private CharacterController characterController; // Thành phần kiểm soát nhân vật
    private float stepTimer = 0f; // Bộ đếm thời gian cho bước chân

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Kiểm tra xem nhân vật có đang di chuyển không
        if (characterController != null && characterController.velocity.magnitude > 0.1f)
        {
            stepTimer += Time.deltaTime;

            // Phát âm thanh sau mỗi bước
            if (stepTimer >= stepInterval)
            {
                audioSource.PlayOneShot(footstepSound);
                stepTimer = 0f; // Đặt lại bộ đếm
            }
        }
        else
        {
            // Dừng phát âm thanh nếu nhân vật không di chuyển
            stepTimer = 0f;
        }
    }
}