/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/11/18
 * 
 * 
 * Nadir Fall State
 * 
 */
using UnityEngine;

public class NadirFallState : NadirBaseState
{
    public NadirFallState(NadirStateMachine stateMachine) : base(stateMachine) { }

    Vector3 fallVelocity;

    public override void Enter()
    {
        stateMachine.action = "Fall";

        stateMachine.mAnimator.Jump();

        this.fallVelocity = Vector3.zero;
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void FixedTick(float deltaTime)
    {
        //Debug.Log("Player Fall");

        //壁にぶつかった
        if (stateMachine.isWalled)
        {
            stateMachine.previousHorizontalVelocity = Vector3.zero;
            stateMachine.mRigid.linearVelocity = new Vector3(0.0f, -2.0f, 0.0f);
        }

        //CameraController.Instance.CameraPositon = mRigid.position;
        //Debug.Log(mRigid.linearVelocity);

        //ジャンプ中の速度
        //stateMachine.mRigid.linearVelocity = new Vector3(stateMachine.previousHorizontalVelocity.x * stateMachine.horizontalJumpFactor
        //                                    , stateMachine.mRigid.linearVelocity.y
        //                                    , stateMachine.previousHorizontalVelocity.z * stateMachine.horizontalJumpFactor);

        float gravity = -9.81f;

        fallVelocity.y += gravity * Time.deltaTime;

        Vector3 moveVector = new Vector3(stateMachine.previousHorizontalVelocity.x * stateMachine.horizontalJumpFactor
                                            , fallVelocity.y
                                            , stateMachine.previousHorizontalVelocity.z * stateMachine.horizontalJumpFactor);

        stateMachine.mCharacterController.Move(moveVector * Time.deltaTime);

        if (stateMachine.isGrounded) { stateMachine.startFallingTime = 0; }

        if (stateMachine.isGrounded)
        {
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.action = "Null";
    }
}
