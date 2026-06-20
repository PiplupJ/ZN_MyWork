using UnityEngine;

public abstract class EnemyState
{
    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();

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
