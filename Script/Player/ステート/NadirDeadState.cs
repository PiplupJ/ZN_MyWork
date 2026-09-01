/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/12/01
 * 
 * 
 * Nadir Dead State
 * 
 */
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class NadirDeadState : NadirBaseState
{
    public NadirDeadState(NadirStateMachine stateMachine) : base(stateMachine) { }

    private float countdown;    

    public override void Enter()
    {
        this.countdown = stateMachine.deathAnimationDuration;
        stateMachine.moveDirection = Vector3.zero;

        stateMachine.action = "Dead";

        stateMachine.mAnimator.Dead();
        stateMachine.transform.GetComponent<CharacterController>().enabled = false;
    }

    public override void Tick(float deltaTime)
    {
        this.countdown -= deltaTime;

        if (countdown <= 0) 
        { 
            stateMachine.DestroyPlayer();
            
        }
    }

    public override void FixedTick(float deltaTime)
    {

    }

    public override void Exit()
    {
        
    }
}
