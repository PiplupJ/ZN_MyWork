//映像終了後、次のシーンをロード
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class VideoSceneLoader : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName = "Title";
    void Start()
    {
        if(videoPlayer == null)
        {
            
        }
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnDestroy() {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }    
    }
}
