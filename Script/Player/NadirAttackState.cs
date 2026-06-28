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
    AttackProfile currentAttack;

    //先行入力状態
    enum InputBuffer
    {
        none, Step, Combo
    }
    InputBuffer buffer;

    //攻撃の現在の段階
    enum AttackPhase
    {
        Ready, Attack, Recover
    }
    AttackPhase phase;

    //現在コンボの何番目か
    int comboIndex;

    //ステート開始
    public override void Enter()
    {
        stateMachine.action = "Attack";

        //武器設定
        if (stateMachine.weapon.TryGetComponent<WeaponController>(out WeaponController Weapon))
        {
            Weapon.hit += WeaponHit;
            Weapon.PowerUpdate(stateMachine.WeaponPower);
        }

        stateMachine.weapon.GetComponent<Collider>().enabled = false;

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
                    comboIndex = (comboIndex + 1)%stateMachine.comboData.attackDatas.Count;
                    StartAttack();
                }
            break;
            case InputBuffer.Step :
                if(CanCancel())
                {
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
        float t = stateMachine.mAnimator.GetNormalizedTime("Attack");

        HandleInput();
        TryBufferedInput();

        switch(phase)
        {
            default :
                break;
            case AttackPhase.Ready :
                //攻撃フレームなら、コライダーを有効化にする
                if(t>= currentAttack.activeFrame)
                {
                    phase = AttackPhase.Attack;
                    stateMachine.weapon.GetComponent<Collider>().enabled = true;
                    PlayerSound.Instance.slash();
                }
                break;
            case AttackPhase.Attack :
                //回復フレームなら、コライダーを無効化にする
                if(t>=currentAttack.recoverFrame)
                {
                    stateMachine.weapon.GetComponent<Collider>().enabled = false;
                    phase = AttackPhase.Recover;
                }
            break;
            case AttackPhase.Recover :
                //予約された先行入力がないと、モーション終了後Idleへ
                if(t>=1.0){
                    stateMachine.SwitchState(new NadirIdleState(stateMachine));
                }
            break;
        }        
    }

    public override void FixedTick(float deltaTime)
    {
    }

    public override void Exit()
    {
        stateMachine.action = "Null";

        stateMachine.mAnimator.ResetAttack();

        //武器の衝突判定によって必殺ゲージが貯めたことを無効化にする
        if (stateMachine.weapon.TryGetComponent<WeaponController>(out WeaponController Weapon))
        {
            Weapon.hit -= WeaponHit;
        }
        //念の為コライダーオフ
        stateMachine.weapon.GetComponent<Collider>().enabled = false;

    }

    //武器が当たる時
    private void WeaponHit()
    {
        stateMachine.ChargeSuperGauge();

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
        currentAttack = stateMachine.comboData.attackDatas[comboIndex];
        stateMachine.mAnimator.PlayMotion(currentAttack.motionTag);
    }
}
