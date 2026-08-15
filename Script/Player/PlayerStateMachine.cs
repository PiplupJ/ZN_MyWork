/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/11/17
 * 
 * 
 * プレイヤーステートマシーン　ベース
 * 
 */
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    protected PlayerState currentPlayerState;

    public void SwitchState(PlayerState newPlayerState)
    {
        if (currentPlayerState != null)
        {
            currentPlayerState.Exit();
        }

        currentPlayerState = newPlayerState;

        if (currentPlayerState != null)
        {
            currentPlayerState.Enter();
        }
    }


    protected void Update()
    {
        if (currentPlayerState != null)
        {
            currentPlayerState.Tick(Time.deltaTime);
        }
    }

    protected void FixedUpdate()
    {
        if (currentPlayerState != null)
        {
            currentPlayerState.FixedTick(Time.fixedDeltaTime);
        }
    }
}
