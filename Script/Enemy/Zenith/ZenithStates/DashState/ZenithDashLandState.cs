using UnityEngine;

public class ZenithDashLandState : ZenithBaseState
{
    public ZenithDashLandState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.DashFinish();
    }

    public override void Tick(float deltaTime)
    {   
        RotateToPlayer(deltaTime);

        if(stateMachine.mAnimator.GetNormalizedTime("Dash")>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
