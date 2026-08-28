using UnityEngine;

public class ZenithPhaseTransitionState : ZenithBaseState
{
    public ZenithPhaseTransitionState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.PhaseTransition();
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("PhaseTransition");

        if (elapsedTime >= 1)
        {
            stateMachine.BattleFinish();
            stateMachine.SwitchState(new ZenithIdleState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
