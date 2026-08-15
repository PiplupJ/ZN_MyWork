/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * First update: 2025/11/29
 * Last update: 2026/06/03 by 張源碩
 * 
 * 
 * Nadir Attack State
 * 
 */
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class NadirAttackState : NadirBaseState
{
    public NadirAttackState(NadirStateMachine stateMachine) : base(stateMachine) { }

    //現在の攻撃
    AttackData currentAttack;

    //先行入力状態
    enum InputBuffer
    {
        none, Step, Combo
    }
    InputBuffer buffer;

    //攻撃の現在の段階
    enum AttackPhase
    {
        Ready, Attack, Recover, End
    }
    AttackPhase phase;

    //現在コンボの何番目か
    int comboIndex;

    //ステート開始
    public override void Enter()
    {
        stateMachine.action = "Attack";

        //武器設定

        stateMachine.weapon.Deactivate();

        comboIndex = 0;

        StartAttack();

    }

    //先行入力を受ける
    void HandleInput()
    {
        if (stateMachine.playerAttack.WasPressedThisFrame())
        {
            buffer = InputBuffer.Combo;
        }

        if (stateMachine.playerStep.WasPressedThisFrame())
        {
            buffer = InputBuffer.Step;
        }

        if (stateMachine.playerMove.IsPressed())
        {
            stateMachine.moveDirection = stateMachine.playerMove.ReadValue<Vector2>();
        }
    }
    //先行入力を適用
    void TryBufferedInput()
    {
        switch(buffer)
        {
            case InputBuffer.Combo :
                if(phase == AttackPhase.Recover){
                    if (comboIndex + 1 < stateMachine.comboData.combo.Count)  
                    {
                        stateMachine.weapon.Deactivate();
                        comboIndex++;
                        StartAttack();
                    }
                    else
                    {
                        buffer = InputBuffer.none; 
                    }
                }
            break;
            case InputBuffer.Step :
                if(CanCancel())
                {   
                    buffer = InputBuffer.none;
                    stateMachine.SwitchState(new NadirStepState(stateMachine));
                }
                break;
            default :
                break;
        }
    }
    //更新
    public override void Tick(float deltaTime)
    {
        float t = stateMachine.mAnimator.GetNormalizedTime(currentAttack.motionTag);

        switch(phase)
        {
            default :
                break;
            case AttackPhase.Ready :
                //攻撃フレームなら、コライダーを有効化にする
                if(t>= currentAttack.activeFrame)
                {
                    phase = AttackPhase.Attack;
                    string se = "N_Swing" + (UnityEngine.Random.Range(0, 3)+1).ToString();
                    SoundPlayer.Instance.PlaySE(se, 1f, Random.Range(-0.2f, 0.2f));
                    stateMachine.weapon.Activate();
                }
                break;
            case AttackPhase.Attack :
                //回復フレームなら、コライダーを無効化にする
                if(t>=currentAttack.recoverFrame)
                {
                    stateMachine.weapon.Deactivate();
                    phase = AttackPhase.Recover;
                }
            break;
            case AttackPhase.Recover :
                //予約された先行入力がないと、モーション終了後Idleへ
                if(t>=1.0f){
                    phase = AttackPhase.End;
                    stateMachine.SwitchState(new NadirIdleState(stateMachine));
                }
            break;
        }        
        HandleInput();
        TryBufferedInput();
    }

    public override void FixedTick(float deltaTime)
    {
    }

    public override void Exit()
    {
        stateMachine.action = "Null";

        //念の為コライダーオフ
        stateMachine.weapon.Deactivate();
        Debug.Log("Attack End");
    }

    //現在攻撃キャンセルができるか
    private bool CanCancel()
    {
        return phase!=AttackPhase.Attack;    
    }
    //攻撃開始（現在の攻撃にコンボデータを適用する)
    protected void StartAttack()
    {
        phase = AttackPhase.Ready;
        buffer = InputBuffer.none;
        currentAttack = stateMachine.comboData.combo[comboIndex];
        stateMachine.weapon.SetAttack(currentAttack.GetAttackInfo(stateMachine.gameObject));
        stateMachine.mAnimator.PlayMotion(currentAttack.motionTag, 0);
        stateMachine.mAnimator.PlayWeaponMotion(currentAttack.motionTag, 0);
    }
}
