using UnityEngine;
using System;

public class EnemyStateMachine : MonoBehaviour
{
    //現在のステート
    protected EnemyState currentEnemyState;

    //1/9追加。シーンマネジャーにボスが倒れたことを知らせる
    public event Action TakenDown;
   
   //ステート変更
    public void SwitchState(EnemyState newEnemyState)
    {
        //実行中のステートがあれば終了
        if(currentEnemyState != null)
        {
            currentEnemyState.Exit();
        }
        //ステート変更
        currentEnemyState = newEnemyState;
        //変更を成功したら開始処理
        if(currentEnemyState != null)
        {
            currentEnemyState.Enter();
        }
    }
    //ステートの更新
    protected void Update()
    {
        if(currentEnemyState!=null)
        {
           currentEnemyState.Tick(Time.deltaTime);
        }
    }
    
    //体力が0になったら、体力クラスによって実行
    public void BattleFinish()
    {
        if(TakenDown!=null)
        {
            TakenDown.Invoke();
        }
        
    }
}
