using UnityEngine;

public class ZenithIdleState : ZenithBaseState
{
    public ZenithIdleState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.Idle();
        stateMachine.mAnimator.SetBlendMotion(0);
    }

    public override void Tick(float deltaTime)
    {
        if(InChaseRange())
        {
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
