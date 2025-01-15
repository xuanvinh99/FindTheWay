using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ketgame : MonoBehaviour
{
    [Header("Zombie bi chon: ")]
    public Zombie1 assignedZombie; // Kéo và thả Zombie vào đây.

    [Header("Video Settings")]
    public VideoPlayer videoPlayer; // Gán VideoPlayer vào đây.
    public GameObject videoUI;      // Kéo và thả Panel VideoUI vào đây.
    public GameObject[] uiElementsToHide; // Mảng các UI cần ẩn khi video xuất hiện.
    public AudioSource audioSource; // Gán AudioSource vào đây.

    private bool hasPlayedVideo = false;

    void Update()
    {
        // Kiểm tra nếu zombie đã chết và video chưa được phát.
        if (assignedZombie == null && !hasPlayedVideo)
        {
            PlayVideoAndMusic();
        }
    }

    private void PlayVideoAndMusic()
    {
        hasPlayedVideo = true;

        // Ẩn tất cả các UI không cần thiết
        foreach (GameObject uiElement in uiElementsToHide)
        {
            uiElement.SetActive(false);
        }

        // Hiển thị VideoUI
        if (videoUI != null)
            videoUI.SetActive(true);

        // Phát video
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += EndGame; // Đăng ký sự kiện khi video kết thúc.
        }

        // Phát nhạc
        if (audioSource != null)
        {
            audioSource.Play(); // Phát nhạc khi video bắt đầu.
        }

        // Dừng thời gian trong game
        Time.timeScale = 0f;
    }

    private void EndGame(VideoPlayer vp)
    {
        // Kết thúc game hoặc chuyển sang màn hình kết thúc
        Debug.Log("Video kết thúc. Kết thúc game.");
        Application.Quit(); // Dùng khi build.
        // SceneManager.LoadScene("EndScene"); // Dùng để chuyển sang một scene khác.
    }
}