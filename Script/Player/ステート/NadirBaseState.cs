/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/11/17
 * 
 * 
 * NADIR Move State
 * 
 */

using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public abstract class NadirBaseState : PlayerState
{
    protected NadirStateMachine stateMachine;

    protected NadirBaseState(NadirStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    //汎用メソッド追加↴

    //インプット
    protected bool Ground_BasicMovement()
    {
        //落下
        if (stateMachine.isFalling)
        {
            stateMachine.SwitchState(new NadirFallState(stateMachine));
            return true;
        }

        //ステップ
        if ((stateMachine.stepBufferFrame > 0 && stateMachine.playerMove.IsPressed()))
        {
            stateMachine.moveDirection = stateMachine.playerMove.ReadValue<Vector2>();
            stateMachine.stepButtonPressedFirst = false;
            stateMachine.stepBufferFrame = 0;
            stateMachine.SwitchState(new NadirStepState(stateMachine));
            return true;
        }
        //ジャンプ
        else if (stateMachine.playerJump.WasPressedThisFrame())
        {
            stateMachine.SwitchState(new NadirJumpState(stateMachine));
            return true;
        }
        //パリー
        else if(stateMachine.playerParry.WasPressedThisFrame())
        {
            stateMachine.SwitchState(new NadirParryState(stateMachine));
            return true;
        }
        //攻撃
        else if (stateMachine.playerAttack.WasPressedThisFrame())
        {
            stateMachine.SwitchState(new NadirAttackState(stateMachine));
            return true;
        }
        //必殺技
        else if (stateMachine.canSuperAttack && stateMachine.playerSuper.WasPressedThisFrame())
        {
            //stateMachine.SwitchState(new NadirSuperStandbyState(stateMachine));
            stateMachine.SwitchState(new NadirSuperCutInState(stateMachine));
            return true;
        }
        //移動
        else if (stateMachine.playerMove.IsPressed())
        {
            if (stateMachine.action == "Move")
            {
                return true;
            }
            stateMachine.SwitchState(new NadirMoveState(stateMachine));
            return true;
        }

        return false;
    }

    //キャラ回転
    public void PlayerRotate(float deltaTime)
    {
        if (stateMachine.moveDirection.sqrMagnitude < 0.01f)
            return;

        switch (stateMachine.rotateMode)
        {
            case NadirStateMachine.RotateMode.Independent:
                PlayerRotate_Independent(deltaTime);
                break;

            case NadirStateMachine.RotateMode.WithCamera:
                PlayerRotate_WithCamera(deltaTime);
                break;
        }
    }

    private void PlayerRotate_Independent(float deltaTime)
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(
            stateMachine.mCamera.transform.forward,
            Vector3.up
        ).normalized;

        Vector3 cameraRight = Vector3.ProjectOnPlane(
            stateMachine.mCamera.transform.right,
            Vector3.up
        ).normalized;

        Vector3 targetDirection =
            cameraForward * stateMachine.moveDirection.y +
            cameraRight * stateMachine.moveDirection.x;

        if (targetDirection.sqrMagnitude < 0.0001f)
            return;

        stateMachine.rotateTarget = Quaternion.LookRotation(targetDirection);

        stateMachine.transform.rotation = Quaternion.RotateTowards(
            stateMachine.transform.rotation,
            stateMachine.rotateTarget,
            stateMachine.rotateSpeed * deltaTime
        );
    }

    private void PlayerRotate_WithCamera(float deltaTime)
    {
        float cameraY = CameraController.Instance.transform.eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0f, cameraY, 0f);

        stateMachine.transform.rotation = Quaternion.RotateTowards(
            stateMachine.transform.rotation,
            targetRotation,
            stateMachine.rotateSpeed * deltaTime
        );
    }

    //目標に向かって回転
    public void PlayerRotateTowards(Transform _target)
    {
        //目標の方向を取得
        Vector3 direction = _target.position - stateMachine.transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        Quaternion lookDirection = Quaternion.LookRotation(direction);

        //回転
        stateMachine.rotateTarget = lookDirection;
        stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation, stateMachine.rotateTarget, stateMachine.rotateSpeed * Time.fixedDeltaTime);
    }

    //ステッププレインプット処理
    public void StepPreInput()
    {
        //ステップできるか?
        if (stateMachine.playerStep.WasPressedThisFrame())
        {
            stateMachine.stepButtonPressedFirst = true;
            stateMachine.stepBufferFrame = stateMachine.stepBufferFrameAmount;
        }
    }

    //ターゲットへの向き
    public bool TryGetDirectionToTarget(GameObject target, out Vector3 dir)
    {
        dir = stateMachine.transform.forward;                    
        if (target == null) return false;           

        Vector3 diff = target.transform.position - stateMachine.transform.position;
        diff.y = 0f;
        if (diff.sqrMagnitude < 0.001f) return false;   

        dir = diff.normalized;
        return true;
    }

    //攻撃する際に敵へ回転
    public void RotateToBoss()
    {
        var boss = BattleSceneManager.CurrentBoss;
        if (boss == null) return;

        PlayerRotateTowards(boss.transform);
    }

    protected enum MovementState
    {
        Stop,
        Walk,
        Run,
        Attack
    }
    protected MovementState movementState;
    protected void UpdateMoveVelocity(float deltaTime)
    {
        Vector2 directionInput = stateMachine.playerMove.ReadValue<Vector2>();

        float inputMagnitude = directionInput.magnitude;

        bool hasInput = inputMagnitude > 0.1f;

        if (hasInput)
        {
            stateMachine.moveDirection = directionInput.normalized;
        }
        else
        {
            stateMachine.moveDirection = Vector2.zero;
        }

        if (!hasInput)
        {
            movementState = MovementState.Stop;
        }
        else if (movementState == MovementState.Run)
        {
            movementState = inputMagnitude > 0.4f
                ? MovementState.Run : MovementState.Walk;
        }
        else
        {
            movementState = inputMagnitude > 0.6f
                ? MovementState.Run : MovementState.Walk;
        }
    }

    protected void ApplyMoveVelocity(float deltaTime)
    {
        //移動量更新
        float targetSpeed;

        switch(movementState)
        {
            case MovementState.Run :
                targetSpeed = stateMachine.runSpeed;
                break;
            case MovementState.Walk :
                targetSpeed = stateMachine.walkSpeed;
                break;
            default :
                targetSpeed = 0;
                break;
        }

        Vector3 targetVelocity = targetSpeed
                                * new Vector3(stateMachine.moveDirection.x, 0, stateMachine.moveDirection.y);

        float accel;
        if(targetVelocity.sqrMagnitude < stateMachine.currentVelocity.sqrMagnitude)
        {
            accel = stateMachine.moveDeceleration;
        }
        else if(Vector3.Dot(targetVelocity, stateMachine.currentVelocity) < 0)
        {
            accel = stateMachine.moveDeceleration;
        }
        else if(movementState == MovementState.Run)
        {
            accel = stateMachine.runAcceleration;
        }
        else
        {
            accel = stateMachine.walkAcceleration;
        }

        stateMachine.currentVelocity = Vector3.MoveTowards(
            stateMachine.currentVelocity, targetVelocity, accel * deltaTime);

        //カメラの向きを取得
        Vector3 cameraForward = Vector3.ProjectOnPlane(stateMachine.mCamera.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(stateMachine.mCamera.transform.right, Vector3.up).normalized;

        Vector3 moveVector = Vector3.zero;

        float gravity = -9.81f;

        //前方
        moveVector += cameraForward * stateMachine.currentVelocity.z;
        
        //横
        moveVector += cameraRight * stateMachine.currentVelocity.x;


        //bool isWalkableSlope = (Vector3.Angle(Vector3.up, stateMachine.hitNormal) <= stateMachine.mCharacterController.slopeLimit);

        //if (isWalkableSlope)
        //{
          //  moveVector = Vector3.ProjectOnPlane(moveVector, stateMachine.hitNormal);
        //}

        //下
        moveVector += Vector3.up * gravity;

        PlayerRotate(deltaTime);

        stateMachine.mCharacterController.Move(moveVector *deltaTime);

        stateMachine.previousHorizontalVelocity = moveVector;
    }

    protected void MoveWithFixedSpeed(float deltaTime, float moveSpeed, Vector3 dir)
    {
        Vector3 movementVector = dir * moveSpeed * deltaTime;
        
        stateMachine.mCharacterController.Move(movementVector);
    }
}
