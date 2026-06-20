using UnityEngine;

public class ZenithIdleState : ZenithBaseState
{
    private readonly int BasicMotionHash = Animator.StringToHash("BasicMotion");

    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public ZenithIdleState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("ZenithIdleState"+Time.time);
        stateMachine.PrevState = ZenithStates.Idle;
        stateMachine.Animator.CrossFadeInFixedTime(BasicMotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(InChaseRange())
        {
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }

        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {

    }
}
