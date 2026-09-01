/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * First update: 2025/11/17
 * Last update : 2026/07/11 by 張源碩(ジャンウォンソク)
 * 
 * Nadir Move State
 * 
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class NadirMoveState : NadirBaseState
{
    private float SFXCD;
    private const float SFXlength = 3.0f;

    enum MovementState
    {
        Stop,
        Walk,
        Run
    }
    MovementState movementState;

    public NadirMoveState(NadirStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.Idle();
        stateMachine.action = "Move";
        SFXCD = 0.9f;
    }

    public override void Tick(float deltaTime)
    {
        Vector2 directionInput = stateMachine.playerMove.ReadValue<Vector2>();

        stateMachine.moveDirection = directionInput.normalized;

        float inputMagnitude = directionInput.magnitude;

        bool hasInput = inputMagnitude > 0.0001f;

        if(hasInput){
            if(inputMagnitude > 0.5f){
                movementState = MovementState.Run;
            }
            else{
                movementState = MovementState.Walk;
            }
        }
        else{
            movementState = MovementState.Stop;
        }

        float targetSpeed;
        float acceleration;

        switch(movementState)
        {
            case MovementState.Run :
                targetSpeed = stateMachine.runSpeed;
                acceleration = stateMachine.runAcceleration;
                break;
            case MovementState.Walk :
                targetSpeed = stateMachine.walkSpeed;
                acceleration = stateMachine.walkAcceleration;
                break;
            default :
                targetSpeed = 0;
                acceleration = stateMachine.moveDeceleration;
                break;
        }

        Vector3 targetVelocity = targetSpeed * new Vector3(stateMachine.moveDirection.x, 0, stateMachine.moveDirection.y);

        if(targetVelocity.sqrMagnitude > stateMachine.currentVelocity.sqrMagnitude){
            stateMachine.currentVelocity = Vector3.MoveTowards(stateMachine.currentVelocity, targetVelocity, acceleration * deltaTime);
        }
        //減速
        else{
            stateMachine.currentVelocity = Vector3.MoveTowards(stateMachine.currentVelocity, targetVelocity, stateMachine.moveDeceleration*deltaTime);
        }

        float currentSpeed = stateMachine.currentVelocity.magnitude; 

        stateMachine.mAnimator.Move(currentSpeed/ stateMachine.runSpeed);

        SFXCD += (currentSpeed/SFXlength)*deltaTime;

        if (SFXCD >= 1f)
        {
            SoundPlayer.Instance.PlaySE("N_Walk01", 0.5f, Random.Range(-0.2f, 0.2f));
            SFXCD -= 1f;
        }

        StepPreInput();

        if (!Ground_BasicMovement()&&currentSpeed<0.0001f)
        {
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
        }
    }

    public override void FixedTick(float deltaTime)
    {
        
        //カメラの向きを取得
        Vector3 cameraForward = Vector3.ProjectOnPlane(stateMachine.mCamera.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(stateMachine.mCamera.transform.right, Vector3.up).normalized;

        Vector3 moveVector = Vector3.zero;

        float gravity = -9.81f;

        //前方
        moveVector += cameraForward * stateMachine.currentVelocity.z;
        

        //横
        moveVector += cameraRight * stateMachine.currentVelocity.x;


        bool isWalkableSlope = (Vector3.Angle(Vector3.up, stateMachine.hitNormal) <= stateMachine.mCharacterController.slopeLimit);

        if (isWalkableSlope)
        {
            moveVector = Vector3.ProjectOnPlane(moveVector, stateMachine.hitNormal);
        }

        //下
        moveVector += Vector3.up * gravity;

        stateMachine.mCharacterController.Move(moveVector * deltaTime);

        PlayerRotate();

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

        stateMachine.previousHorizontalVelocity = moveVector;

    }

    public override void Exit()
    {
        stateMachine.action = "Null";
    }
}
