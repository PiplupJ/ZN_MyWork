/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * First update: 2025/11/17
 * Last update : 2026/07/11 by 張源碩(ジャンウォンソク)
 * 
 * Nadir Idle State
 * 
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class NadirIdleState : NadirBaseState
{
    public NadirIdleState(NadirStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.action = "Idle";
       
        //stateMachine.mRigid.linearVelocity = Vector3.up * -0.5f + Vector3.zero;
        stateMachine.previousHorizontalVelocity = Vector3.zero;

        stateMachine.mAnimator.Idle();
        stateMachine.mAnimator.Move(0);
    }

    public override void Tick(float deltaTime)
    {
       
        StepPreInput();
        Ground_BasicMovement();
    }

    public override void FixedTick(float deltaTime)
    {
        //Debug.Log("Player Idle");

        //ステッププレインプット
        if (stateMachine.stepBufferFrame > 0)
        {
            stateMachine.stepBufferFrame--;

            if (stateMachine.stepBufferFrame <= 0)
            {
                stateMachine.moveDirection = Vector2.zero;
                stateMachine.stepButtonPressedFirst = false;

            }
        }
        stateMachine.moveDirection = Vector3.zero;
    }

    public override void Exit()
    {
        stateMachine.action = "Null";
    }
}
