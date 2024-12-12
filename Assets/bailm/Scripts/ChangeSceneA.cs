using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ChangeSceneA : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Start()
    {
        // Đảm bảo videoPlayer không phải null
        if (videoPlayer != null)
        {
            // Đăng ký sự kiện khi video kết thúc
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("VideoPlayer không được gán trong VideoController.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Scene_B");
    }
}
