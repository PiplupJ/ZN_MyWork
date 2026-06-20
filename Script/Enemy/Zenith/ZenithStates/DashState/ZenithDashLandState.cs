using UnityEngine;

public class ZenithDashLandState : ZenithBaseState
{
    public ZenithDashLandState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    protected readonly int DashHash3 = Animator.StringToHash("Dash_Land");

    protected const float TransitionDuration = 0.1f;

    public override void Enter()
    {
        Debug.Log("ZenithDashLanded"+Time.time);
        stateMachine.Animator.CrossFadeInFixedTime(DashHash3, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {   
        RotateToPlayer(deltaTime);

        if(GetNormalizedTime(stateMachine.Animator, "Dash_Land")>=1){
            stateMachine.SwitchState(new ZenithAttackingState(stateMachine));
            //stateMachine.SwitchState(new ZenithFirstMeleeAttackState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
