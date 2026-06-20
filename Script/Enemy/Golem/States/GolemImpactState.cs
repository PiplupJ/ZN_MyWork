using UnityEngine;

public class GolemImpactState : GolemBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");

    private const float CrossFadeDuration = 0.1f;

    public GolemImpactState(GolemStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.sem.playEnemySE(EnemySEtype.GolemImpact);
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator, "Impact")>=1){
            stateMachine.SwitchState(new GolemBreakState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
