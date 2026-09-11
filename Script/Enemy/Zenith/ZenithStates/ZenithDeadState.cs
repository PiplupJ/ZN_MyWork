using UnityEngine;

public class ZenithDeadState : ZenithBaseState
{
    public ZenithDeadState(ZenithStateMachine stateMachine) : base(stateMachine) { }
    
    enum DeathPhase
    {
        Enter, OnGoing, Finished
    }
    DeathPhase deathPhase;
    public override void Enter()
    {
        stateMachine.mAnimator.Death();
        deathPhase = DeathPhase.OnGoing;
    }


    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Death");

        if (elapsedTime >= 1 && deathPhase != DeathPhase.Finished)
        {
            deathPhase = DeathPhase.Finished;
            stateMachine.DestroyZenith();
            return;
        }
    }

    public override void Exit()
    {

    }
}
