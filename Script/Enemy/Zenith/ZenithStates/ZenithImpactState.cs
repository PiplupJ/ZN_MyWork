using UnityEngine;

public class ZenithImpactState : ZenithBaseState
{
    public ZenithImpactState(ZenithStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("ImpactState!");
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.BattleFinish();
        //stateMachine.SwitchState(new ZenithIdleState(stateMachine));
        return;

    }

    public override void Exit()
    {

    }
}
