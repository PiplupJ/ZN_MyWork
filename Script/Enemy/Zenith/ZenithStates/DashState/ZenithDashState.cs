using UnityEngine;

public class ZenithDashState : ZenithBaseState
{
    public ZenithDashState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    protected readonly int DashHash2 = Animator.StringToHash("Dash_Move");

    protected const float TransitionDuration = 0.1f;

    public override void Enter()
    {
        Debug.Log("ZenithDashToNadir!"+Time.time);
        stateMachine.Animator.CrossFadeInFixedTime(DashHash2, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {   
        RotateToPlayer(deltaTime);
        
        MoveToPlayer(deltaTime, stateMachine.dashSpeed);

        if(InMeleeAttackRange()==true){
            stateMachine.SwitchState(new ZenithDashLandState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
