using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// デバッグ用のシーン切り替えスクリプト
/// Ctrl + 1～5でシーンを切り替えることができる
/// 1 :タイトル、２：チュートリアル、３：ゴーレム、４：ゼニス１、５：ゼニス２
public class DebugSceneSwitcher : MonoBehaviour
{
    struct SceneInfo
    {
        public string sceneName;
        public KeyCode numKey;
        public KeyCode keyPad;
        public SceneInfo(string sceneName, KeyCode numKey, KeyCode keyPad)
        {
            this.sceneName = sceneName;
            this.numKey = numKey;
            this.keyPad = keyPad;
        }
    }

    private SceneInfo[] sceneInfo = new SceneInfo[]
    {
        new SceneInfo("Title", KeyCode.Alpha1, KeyCode.Keypad1),
        new SceneInfo("TutorialStage", KeyCode.Alpha2, KeyCode.Keypad2),
        new SceneInfo("G01", KeyCode.Alpha3, KeyCode.Keypad3),
        new SceneInfo("Z01", KeyCode.Alpha4, KeyCode.Keypad4),
        new SceneInfo("Z02", KeyCode.Alpha5, KeyCode.Keypad5)
    };

    void Update()
    {
        //ctrlが押されていないとリターン
        if(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            return;
        }
        //押したキー入力に応じてシーンを切り替える
        for (int i = 0; i < sceneInfo.Length; i++)
        {
            if (Input.GetKeyDown(sceneInfo[i].numKey) || Input.GetKeyDown(sceneInfo[i].keyPad))
            {
                SceneManager.LoadScene(sceneInfo[i].sceneName);
            }
        }
    }
}
