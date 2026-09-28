using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BattleSceneManager : MonoBehaviour
{
    public static EnemyStateMachine CurrentBoss { get; private set; } //外部から読み込み用
    EnemyStateMachine esm;
    [SerializeField] private float timer = 3.0f;
    [SerializeField] private string NextSceneString;
    private bool onProcess = false;

    public void InitBattleSceneManager(EnemyStateMachine e)
    {
        if(esm==null)
        {
            esm = e;
            esm.TakenDown += HandleBossTakenDown;
            CurrentBoss = e;
        }
    }

    private void OnDisable()
    {
        if(esm!=null)
        {
            esm.TakenDown -= HandleBossTakenDown;
        }
        if (CurrentBoss == esm)
        {
            CurrentBoss = null;
        }
            
    }

    private void HandleBossTakenDown()
    {
        if (onProcess) { return; }//すでに実行中
        onProcess = true;
        StartCoroutine(LoadNextScene(timer));
    }

    private IEnumerator LoadNextScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if(!string.IsNullOrEmpty(NextSceneString))
        {
            SceneManager.LoadScene(NextSceneString);
        }
    }

    
}
