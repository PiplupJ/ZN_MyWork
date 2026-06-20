using UnityEngine;
using System.Collections.Generic;

public class ZenithMeleeAttackState : ZenithAttackingState
{
    public ZenithMeleeAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private float keyValue = 0.5f;

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        RotateToPlayer(deltaTime);
        if(stateMachine.phase != ZenithPhase.Phase2){
            if(Random.value > keyValue){
                stateMachine.SwitchState(new ZenithFirstMeleeAttackState(stateMachine));
                return;
            }
            else    
                stateMachine.SwitchState(new ZenithSecondMeleeAttackState(stateMachine));
                return;
        }
        else{
            stateMachine.SwitchState(new ZenithThirdMeleeAttackState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
