using UnityEngine;

public class ZenithDeadState : ZenithBaseState
{
    public ZenithDeadState(ZenithStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("DeadState!");
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.DestroyZenith();
        stateMachine.SwitchState(new ZenithIdleState(stateMachine));
        return;

    }

    public override void Exit()
    {

    }
}
