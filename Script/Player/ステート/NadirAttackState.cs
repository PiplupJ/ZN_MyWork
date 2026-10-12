/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * First update: 2025/11/29
 * Last update: 2026/06/03 by 張源碩
 * -攻撃データをAttackData(Scriptable Object)で外部化
 * -先行入力バッファとコンボ段数管理を追加
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
        none, Step, Combo, Move
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
        if (stateMachine.playerMove.IsPressed())
        {
            if (buffer != InputBuffer.Combo)
            {
                buffer = InputBuffer.Move;
            }  
        }
        //回避＞コンボ順で優先するo
        if (stateMachine.playerAttack.WasPressedThisFrame())
        {
            buffer = InputBuffer.Combo;
        }

        if (stateMachine.playerStep.WasPressedThisFrame())
        {
            buffer = InputBuffer.Step;
        }

 
    }
    //先行入力を適用
    void TryBufferedInput()
    {
        switch(buffer)
        {
            case InputBuffer.Combo :
                //モーション終了を待たずに、次のコンボ実行
                if(phase == AttackPhase.Recover){
                    if (comboIndex + 1 < stateMachine.comboData.combo.Count)  
                    {
                        stateMachine.weapon.Deactivate();
                        comboIndex++;
                        StartAttack();
                    }
                    //最後のコンボだったら、処理しない
                    else
                    {
                        buffer = InputBuffer.none; 
                    }
                }
            break;
                //回避入力
            case InputBuffer.Step :
                //キャンセルできるならキャンセルして回避を実行
                if(CanCancel())
                {   
                    buffer = InputBuffer.none;
                    stateMachine.SwitchState(new NadirStepState(stateMachine));
                }
                break;
            case InputBuffer.Move:
                if (phase == AttackPhase.Recover)
                {
                    buffer = InputBuffer.none;
                    stateMachine.SwitchState(new NadirMoveState(stateMachine));
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

        UpdateMoveVelocity(deltaTime);

        switch (phase)
        {
            default:
                break;
            case AttackPhase.Ready:
                //攻撃フレームなら、コライダーを有効化にする
                if(t>= currentAttack.activeFrame)
                {
                    phase = AttackPhase.Attack;
                    string se = "N_Swing" + (UnityEngine.Random.Range(0, 3)+1).ToString();
                    SoundPlayer.Instance.PlaySE(se, 1f, Random.Range(-0.2f, 0.2f));
                    stateMachine.weapon.Activate();
                    stateMachine.weapon.weaponFX.Begin(); //エフェクト開始
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
        ApplyMoveVelocity(deltaTime);
        float speed = stateMachine.currentVelocity.magnitude;
        stateMachine.mAnimator.Move(speed/stateMachine.runSpeed);

        stateMachine.mAnimator.SetLowerBodyWeight(speed/stateMachine.walkSpeed);

        switch (phase)
        {
            case AttackPhase.Ready:
                //向き入力があればその向きへ
                if (stateMachine.moveDirection != Vector2.zero)
                {
                    PlayerRotate();
                }
                //ないならボスへ自動回転
                else
                {
                    RotateToBoss();
                }
                break;
            default:
                break;
        }
    }

    public override void Exit()
    {
        stateMachine.action = "Null";

        //念の為コライダーオフ
        stateMachine.weapon.Deactivate();
        stateMachine.mAnimator.SetLowerBodyWeight(0f);
        stateMachine.weapon.weaponFX.End(); //攻撃のエフェクト終了
    }

    //現在攻撃キャンセルができるか
    private bool CanCancel()
    {
        return phase!=AttackPhase.Attack;    
    }
    //攻撃開始（現在の攻撃にコンボデータを適用する)
    protected void StartAttack()
    {
        stateMachine.weapon.weaponFX.End(); //前の攻撃のエフェクト終了
        phase = AttackPhase.Ready; //攻撃段階は準備
        buffer = InputBuffer.none; //先行入力はなし
        currentAttack = stateMachine.comboData.combo[comboIndex]; //次の攻撃情報の読み込み
        //攻撃情報を基にしたダメージ、モーション設定
        stateMachine.weapon.SetAttack(currentAttack.GetAttackInfo(stateMachine.gameObject)); 
        stateMachine.mAnimator.PlayMotion(currentAttack.motionTag, 0);
        //stateMachine.mAnimator.PlayWeaponMotion(currentAttack.motionTag, 0);

        stateMachine.currentVelocity = Vector3.ClampMagnitude(stateMachine.currentVelocity, GetMoveSpeedOnAttack(currentAttack.type));
    }

    float GetMoveSpeedOnAttack(AttackType attackType)
    {
        float moveSpeedOnAttack = 0;
        switch(attackType)
        {
            case AttackType.Light:
                moveSpeedOnAttack = 3.0f;
            break;
            case AttackType.Heavy:
                moveSpeedOnAttack = 1.0f;
            break;
            default : 
            break;
        }
        return moveSpeedOnAttack;
    }
}
