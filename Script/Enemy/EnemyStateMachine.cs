using UnityEngine;
using System;

public class EnemyStateMachine : MonoBehaviour
{
    protected EnemyState currentEnemyState;

    //1/9追加
    public event Action TakenDown;
   
    public void SwitchState(EnemyState newEnemyState)
    {
        if(currentEnemyState != null)
        {
            currentEnemyState.Exit();
        }

        currentEnemyState = newEnemyState;

        if(currentEnemyState != null)
        {
            currentEnemyState.Enter();
        }
    }
    protected void Update()
    {
        if(currentEnemyState!=null)
        {
           currentEnemyState.Tick(Time.deltaTime);
        }
    }
    
    public void BattleFinish()
    {
        if(TakenDown!=null)
        {
            TakenDown.Invoke();
        }
        
    }
}
