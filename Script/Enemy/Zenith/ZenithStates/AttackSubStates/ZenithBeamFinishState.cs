using UnityEngine;

public class ZenithBeamFinishState : ZenithBaseState
{
    public ZenithBeamFinishState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.BeamFinish();
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.mAnimator.GetNormalizedTime("Attack")>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
