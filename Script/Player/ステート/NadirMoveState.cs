/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * First update: 2025/11/17
 * Last update : 2026/07/11 by 張源碩(ジャンウォンソク)
 * -移動の加減速追加
 * -歩き・走りの分離
 * Nadir Move State
 * 
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class NadirMoveState : NadirBaseState
{
    private float SFXCD;
    private const float SFXlength = 3.0f;

    public NadirMoveState(NadirStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.Idle();
        stateMachine.action = "Move";
        SFXCD = 0.9f;
    }

    public override void Tick(float deltaTime)
    {

        float animatorSpeed;
        if (this.movementState == MovementState.Run)
        {
            animatorSpeed = 1.0f;
        }
        else if(this.movementState == MovementState.Walk){
            animatorSpeed = 0.5f;
        }
        else
        {
            animatorSpeed = 0;
        }

        stateMachine.mAnimator.Move(animatorSpeed);
        UpdateMoveVelocity(deltaTime);
        ApplyMoveVelocity(deltaTime);

        float currentSpeed = stateMachine.currentVelocity.magnitude * deltaTime;
        SFXCD += (currentSpeed/SFXlength);

        if (SFXCD >= 1.5f)
        {
            SoundPlayer.Instance.PlaySE("N_Walk01", 0.5f, Random.Range(-0.2f, 0.2f));
            SFXCD -= 1.5f;
        }

        StepPreInput();

        if (!Ground_BasicMovement()&&currentSpeed<0.0001f)
        {
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
        }
    }

    public override void FixedTick(float deltaTime)
    {

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

    }

    public override void Exit()
    {
        stateMachine.action = "Null";
    }
}
