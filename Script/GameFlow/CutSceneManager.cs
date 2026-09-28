using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private string NextSceneString;
    public PlayableDirector director;
    
    private void LoadNextScene()
    {
        if(NextSceneString != null)
            {
                SceneManager.LoadScene(NextSceneString);
            }
    }

    void OnEnable()
    {
        director.stopped += OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
            LoadNextScene();
    }

    void OnDisable()
    {
        director.stopped -= OnTimelineStopped;
    }
}
