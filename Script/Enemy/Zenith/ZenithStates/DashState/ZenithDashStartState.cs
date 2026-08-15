using UnityEngine;

public class ZenithDashStartState : ZenithBaseState
{
    public ZenithDashStartState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.DashStart();
    }

    public override void Tick(float deltaTime)
    {   
        RotateToPlayer(deltaTime);

        if(stateMachine.mAnimator.GetNormalizedTime("Dash")>=1){
            stateMachine.SwitchState(new ZenithDashState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
