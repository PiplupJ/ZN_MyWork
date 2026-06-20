using UnityEngine;

public class ZenithDashStartState : ZenithBaseState
{
    public ZenithDashStartState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    protected readonly int DashHash1 = Animator.StringToHash("Dash_Start");

    protected const float TransitionDuration = 0.1f;

    public override void Enter()
    {
        Debug.Log("ZenithDashState"+Time.time);
        stateMachine.CoolManager.DashCoolDownOn();
        stateMachine.PrevState = ZenithStates.Dash;
        stateMachine.Animator.CrossFadeInFixedTime(DashHash1, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {   
        RotateToPlayer(deltaTime);

        if(GetNormalizedTime(stateMachine.Animator, "Dash_Start")>=1){
            stateMachine.SwitchState(new ZenithDashState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
