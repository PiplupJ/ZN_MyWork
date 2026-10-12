using UnityEngine;

public class GolemImpactState : GolemBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");

    private const float CrossFadeDuration = 0.1f;

    KnockbackEntry entry;
    Vector3 pushDir;
    float speed;
    public GolemImpactState(GolemStateMachine stateMachine, KnockbackEntry entry) : base(stateMachine)
    {
        this.entry = entry;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        if(TryGetDirToPlayer(out pushDir))
        {
            pushDir = -pushDir;
        }
        speed = entry.initialSpeed;
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator, "Impact")>=1){
            stateMachine.SwitchState(new GolemIdleState(stateMachine));
            return;
        }
    }

    public override void FixedTick(float deltaTime)
    {
        GolemMove(deltaTime, speed, pushDir);

        speed = Mathf.Max(0, speed - entry.deceleration*deltaTime);
    }

    public override void Exit()
    {

    }
}
