/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/11/17
 * 
 * 
 * プレイヤーステート　ベース
 * 
 */
using UnityEngine;

public abstract class PlayerState
{
    //ステートに入る
    public abstract void Enter();

    //ステート中の挙動
    public abstract void Tick(float deltaTime);

    public abstract void FixedTick(float deltaTime);

    //ステートから離れる
    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag("Attack"))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag("Attack"))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }


}