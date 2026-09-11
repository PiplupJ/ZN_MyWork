using UnityEngine;

public class ZenithPhaseTransitionState : ZenithBaseState
{
    public ZenithPhaseTransitionState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    enum TransitionState
    {
        Enter, OnGoing, Finished
    }
    TransitionState transitionState;
    public override void Enter()
    {
        stateMachine.mAnimator.PhaseTransition();
        transitionState = TransitionState.OnGoing;
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("PhaseTransition");

        if (elapsedTime >= 1&&transitionState!=TransitionState.Finished)
        {
            stateMachine.BattleFinish();
            transitionState = TransitionState.Finished;
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
