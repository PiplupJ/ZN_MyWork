using UnityEngine;

public class ZenithDeadState : ZenithBaseState
{
    public ZenithDeadState(ZenithStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        stateMachine.mAnimator.Death();
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Death");

        if (elapsedTime >= 1)
        {
            stateMachine.DestroyZenith();
            return;
        }
    }

    public override void Exit()
    {

    }
}
