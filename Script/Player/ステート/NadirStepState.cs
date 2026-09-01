/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/11/18
 * 
 * 
 * Nadir Step State
 * 
 */
using UnityEngine;

public class NadirStepState : NadirBaseState
{
    public NadirStepState(NadirStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.action = "Step";

        //stateMachine.moveDirection = stateMachine.playerMove.ReadValue<Vector2>();

        Vector2 inputDirection = stateMachine.moveDirection.normalized;

        Vector3 cameraForward = Vector3.ProjectOnPlane(stateMachine.mCamera.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(stateMachine.mCamera.transform.right, Vector3.up).normalized;

        Vector3 stepDir = Vector3.zero;

        stepDir += cameraForward * inputDirection.y;
        stepDir += cameraRight * inputDirection.x;
        stepDir = stepDir.normalized;

        Vector3 stepSpeed = stepDir * stateMachine.stepSpeed;

        stateMachine.stepDestination = stateMachine.transform.position + stepSpeed;
        stateMachine.stepFrames = stateMachine.stepTotalFrames;


        //アニメーション

        Vector3 animationDirection = stateMachine.transform.InverseTransformDirection(stepDir).normalized;

        float forward = animationDirection.z;
        float right = animationDirection.x;

        stateMachine.mAnimator.Step(forward, right);
        SoundPlayer.Instance.PlaySE("N_Step01", 1f, Random.Range(-0.2f, 0.2f));

        Debug.Log(forward.ToString() + ", " + right.ToString());

        stateMachine.moveDirection = Vector2.zero;

    }

    public override void Tick(float deltaTime)
    {
        //Debug.Log("Player Step");
    }

    public override void FixedTick(float deltaTime)
    {
        if (stateMachine.stepFrames > 0)
        {
            stateMachine.stepFrames--;

            Vector3 moveVector = Vector3.zero;
            float gravity = -9.81f;

            if (stateMachine.stepFrames > stateMachine.stepTotalFrames - stateMachine.stepTranslateFrames)
            {
            
                moveVector += stateMachine.stepDestination - stateMachine.transform.position;
                moveVector =  moveVector.normalized;
                moveVector *= stateMachine.stepSpeed;


                bool isWalkableSlope = (Vector3.Angle(Vector3.up, stateMachine.hitNormal) <= stateMachine.mCharacterController.slopeLimit);

                if (isWalkableSlope)
                {
                    moveVector = Vector3.ProjectOnPlane(moveVector, stateMachine.hitNormal);
                }

                moveVector += Vector3.up * -9.81f;

                moveVector = moveVector * Time.deltaTime;

                stateMachine.mCharacterController.Move(moveVector);

            }
            else
            {
                stateMachine.mCharacterController.Move( new Vector3(0, gravity * Time.deltaTime, 0));
                stateMachine.mAnimator.Move(0);
            }

        }
        else
        {
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
        }

        //PlayerRotate();
    }

    public override void Exit()
    {
        stateMachine.action = "Null";
    }
}
