using UnityEngine;

public abstract class EnemyState
{
    //ステート開始
    public abstract void Enter();
    //ステート実行
    public abstract void Tick(float deltaTime);
    //ステート終了
    public abstract void Exit();
    //モーション状況返却
    protected float GetNormalizedTime(Animator animator, string tagName)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tagName))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tagName))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

   
}
