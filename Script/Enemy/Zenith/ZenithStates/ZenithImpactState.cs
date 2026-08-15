using UnityEngine;

public class ZenithImpactState : ZenithBaseState
{
    public ZenithImpactState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.Impact();
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Impact");

        Debug.Log("ImpactTime"+elapsedTime);

        if(elapsedTime>=1.0f){
            stateMachine.SwitchState(new ZenithIdleState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
