using UnityEngine;

public class GolemIdleState : GolemBaseState
{
    private readonly int BasicMotionHash = Animator.StringToHash("BasicMotion");

    private readonly int SpeedXHash = Animator.StringToHash("SpeedX");
    private readonly int SpeedYHash = Animator.StringToHash("SpeedY");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public GolemIdleState(GolemStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BasicMotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(InChaseRange())
        {
            stateMachine.SwitchState(new GolemChasingState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(SpeedXHash, 0f, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(SpeedYHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {

    }
}
