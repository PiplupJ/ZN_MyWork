/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/11/18
 * 
 * 
 * Nadir Jump State
 * 
 */
using UnityEngine;

public class NadirJumpState : NadirBaseState
{
    public NadirJumpState(NadirStateMachine stateMachine) : base(stateMachine) { }

    private Vector3 jumpVelocity;
    private float gravity = -9.81f;


    public override void Enter()
    {
        stateMachine.action = "Jump";

        stateMachine.startFallingTime = stateMachine.startFallingTimeMax + 1.0f;
        //mRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //stateMachine.mRigid.linearVelocity = new Vector3(stateMachine.previousHorizontalVelocity.x, stateMachine.jumpSpeed, stateMachine.previousHorizontalVelocity.z);

        this.jumpVelocity = new Vector3(0, stateMachine.jumpSpeed, 0);

        stateMachine.jumpFrames = stateMachine.jumpFrameAmount;

        stateMachine.mAnimator.Jump();

        //Debug.Log("Player Jump");

    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void FixedTick(float deltaTime)
    {
        jumpVelocity.y += gravity * Time.deltaTime;


        stateMachine.jumpFrames--;
        if (stateMachine.jumpFrames <=0 && stateMachine.isGrounded) {
            stateMachine.SwitchState(new NadirIdleState(stateMachine));

        }

        Vector3 moveVector = new Vector3(stateMachine.previousHorizontalVelocity.x * stateMachine.horizontalJumpFactor
                                            , jumpVelocity.y
                                            , stateMachine.previousHorizontalVelocity.z * stateMachine.horizontalJumpFactor);

        stateMachine.mCharacterController.Move(moveVector * Time.deltaTime);
    }

    public override void Exit()
    {
        stateMachine.action = "Null";

    }
}
