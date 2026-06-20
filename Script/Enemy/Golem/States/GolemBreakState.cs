using UnityEngine;

public class GolemBreakState : GolemBaseState
{
    private readonly int BreakHash = Animator.StringToHash("Break");

    private const float CrossFadeDuration = 0.1f;

    public GolemBreakState(GolemStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BreakHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator, "Break")>=1){
            stateMachine.health.ResetImpactCount();
            stateMachine.SwitchState(new GolemIdleState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
