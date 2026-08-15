/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/12/01
 * Last update: 2026/7/11 by ジャンウォンソク
 * 
 * Nadir Super Attack State
 * 
 */
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class NadirSuperStandbyState : NadirBaseState
{
    public NadirSuperStandbyState(NadirStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if(stateMachine.superTarget==null){
            stateMachine.superTarget = GameObject.FindGameObjectWithTag("SuperAttackTarget");
        } 

        stateMachine.moveDirection = Vector3.zero;

        stateMachine.action = "Super Standby";

        CameraController.Instance.SetSuperAttack();
        stateMachine.mAnimator.SuperStandby();
        SoundPlayer.Instance.PlaySE("N_LoadGun01");

    }

    public override void Tick(float deltaTime)
    {
        PlayerHP.instance.SetMuteki();

        //敵が消滅されたら必殺技ステートからぬける
        if (stateMachine.superTarget == null)
        {
            stateMachine.ResetSuperGauge();
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
            return;
        }

        //長押しで弾を発射
        if (stateMachine.playerSuper.IsPressed() && stateMachine.mAnimator.isSuperStandbyEnded())
        {
            stateMachine.SwitchState(new NadirSuperAttackingState(stateMachine));
        }
    }

    public override void FixedTick(float deltaTime)
    {
        PlayerRotateTowards(CameraController.Instance.currentTarget.transform);
    }

    public override void Exit()
    {
        stateMachine.action = "Null";

        CameraController.Instance.SetNormal();
    }


    //敵死亡処理
    private void EnemyExecuted()
    {
        stateMachine.ResetSuperGauge();

        //敵が消滅されたら必殺技ステートからぬける
        stateMachine.SwitchState(new NadirIdleState(stateMachine));
        return;
    }
}
