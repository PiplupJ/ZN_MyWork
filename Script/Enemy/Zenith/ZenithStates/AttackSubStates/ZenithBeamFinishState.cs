using UnityEngine;

public class ZenithBeamFinishState : ZenithAttackingState
{
    public ZenithBeamFinishState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BeamAttackHash2, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator, "Attack_Beam_Finish")>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
