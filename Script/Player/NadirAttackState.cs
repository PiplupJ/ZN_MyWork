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
using UnityEngine.Rendering;

public class NadirAttackState : NadirBaseState
{
    public NadirAttackState(NadirStateMachine stateMachine) : base(stateMachine) { }

    private bool stepPreinput;

    AttackProfile currentAttack;

    int comboIndex;

    bool hasCombo;

    public override void Enter()
    {
        stateMachine.action = "Attack";
        
        stateMachine.attackFrames = stateMachine.attackTotalFrames;

        comboIndex = 0;

        this.stepPreinput = false;

        if (stateMachine.weapon.TryGetComponent<WeaponController>(out WeaponController Weapon))
        {
            Weapon.hit += WeaponHit;
        }

        stateMachine.weapon.GetComponent<Collider>().enabled = false;

        StartAttack();

    }

    public override void Tick(float deltaTime)
    {
        float t = stateMachine.mAnimator.GetNormalizedTime("Attack");

        if (stateMachine.attackFrames > 0)
        {
            //攻撃キャンセル
            if (stateMachine.playerStep.WasPressedThisFrame())
            {
                if (stateMachine.canCancelAttack)
                {
                    stateMachine.SwitchState(new NadirStepState(stateMachine));
                }
                else
                {
                    stepPreinput = true;
                }
            }

            if (stateMachine.playerMove.IsPressed())
            {
                stateMachine.moveDirection = stateMachine.playerMove.ReadValue<Vector2>();
            }

            if (stateMachine.playerAttack.WasPressedThisFrame())
            {
                hasCombo = true;
            }

            if(hasCombo){
            
            if(t >= currentAttack.recoverFrame)
            {
                stateMachine.attackFrames = stateMachine.attackTotalFrames;
                comboIndex = (comboIndex + 1)%stateMachine.comboData.attackDatas.Count;
                StartAttack();
            }   
            }
        return;
        }

        if(comboIndex==stateMachine.comboData.attackDatas.Count-1){
            if(t<1.0){
                return;
            }
        }

        stateMachine.SwitchState(new NadirIdleState(stateMachine));
        
        
    }

    public override void FixedTick(float deltaTime)
    {
        if (stateMachine.attackFrames > 0)
        {
            stateMachine.attackFrames--;
            //stateMachine.mRigid.linearVelocity = new Vector3(0, -0.5f, 0);

            //攻撃モーション後リカバーフレーム
            if (stateMachine.attackFrames <= stateMachine.attackRecoverFrames)
            {
                if (stepPreinput){
                    stateMachine.SwitchState(new NadirStepState(stateMachine));
                }
            }

            //攻撃モーション中フレーム
            else if (stateMachine.attackFrames == stateMachine.attackTotalFrames - stateMachine.attackCancelFrames)
            {
                //Debug.Log("enabled");[
                stateMachine.weapon.GetComponent<Collider>().enabled = true;
                PlayerSound.Instance.slash();
            }
            else if (stateMachine.attackFrames < stateMachine.attackTotalFrames - stateMachine.attackCancelFrames)
            {   
                //ジャンが追加しました。
                //武器の攻撃力を現在の攻撃力でアップデートします。
                if(stateMachine.weapon.TryGetComponent<WeaponController>(out WeaponController Weapon))    
                {
                    Weapon.PowerUpdate(stateMachine.WeaponPower);
                }
            }

            //攻撃モーション前キャンセルフレーム
            else
            {
                stateMachine.weapon.GetComponent<Collider>().enabled = false;
                if (stepPreinput)
                {
                    stateMachine.SwitchState(new NadirStepState(stateMachine));
                }
            }

        }
    }

    public override void Exit()
    {
        stateMachine.action = "Null";

        stateMachine.mAnimator.ResetAttack();

        if (stateMachine.weapon.TryGetComponent<WeaponController>(out WeaponController Weapon))
        {
            Weapon.hit -= WeaponHit;
        }

        stateMachine.weapon.GetComponent<Collider>().enabled = false;

    }

    //武器が当たる時
    private void WeaponHit()
    {
        stateMachine.ChargeSuperGauge();

    }

    protected void StartAttack()
    {
        hasCombo = false;
        currentAttack = stateMachine.comboData.attackDatas[comboIndex];
        stateMachine.mAnimator.PlayMotion(currentAttack.motionTag);
    }
}
