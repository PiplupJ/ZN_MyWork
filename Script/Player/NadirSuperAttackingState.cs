/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2026/07/13 by ジャンウォンソク
 * 
 * 
 * Nadir Super Attack State
 * 
 */
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class NadirSuperAttackingState : NadirBaseState
{
    public NadirSuperAttackingState(NadirStateMachine stateMachine) : base(stateMachine) { }

    private float fireCountdown;

    private AttackInfo superAttackInfo;

    enum SuperAttackPhase
    {
        OnStay,
        OnAttack
    }
    SuperAttackPhase phase;

    public override void Enter()
    {
        stateMachine.moveDirection = Vector3.zero;

        stateMachine.action = "Super Attacking";

        stateMachine.bulletRate = 2f;
        fireCountdown = 0f;

        superAttackInfo = stateMachine.superAttackData.GetAttackInfo(stateMachine.gameObject);

        CameraController.Instance.SetSuperAttack();
        stateMachine.mAnimator.SuperAttacking();

        phase = SuperAttackPhase.OnAttack;
    }

    public override void Tick(float deltaTime)
    {
        PlayerHP.instance.SetMuteki();

        if (stateMachine.superTarget == null)
        {
            stateMachine.ResetSuperGauge();
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
            return;
        }

        switch(phase)
        {
            case SuperAttackPhase.OnAttack :
                if (!stateMachine.playerSuper.IsPressed())
                    phase = SuperAttackPhase.OnStay;
                    stateMachine.mAnimator.SuperStay();
                    break;
            case SuperAttackPhase.OnStay :
                if (stateMachine.playerSuper.IsPressed() && stateMachine.mAnimator.isSuperStandbyEnded())
                    phase = SuperAttackPhase.OnAttack;
                    stateMachine.mAnimator.SuperAttacking();
                    break;
            default :
                break;
        }
    }

    public override void FixedTick(float deltaTime)
    {
        switch(phase)
        {
            case SuperAttackPhase.OnAttack :
                FireSuperAttack();
                break;
            case SuperAttackPhase.OnStay :
                PlayerRotateTowards(CameraController.Instance.currentTarget.transform);
                break;
            default :
                break;
        }
    }

    private void FireSuperAttack()
    {
        if (CameraController.Instance.currentTarget != null)
        {
            PlayerRotateTowards(CameraController.Instance.currentTarget.transform);
        }
        //弾丸の発射
        {
            stateMachine.bulletRate += stateMachine.bulletRateMax / stateMachine.timeToReachBulletRateMax * Time.deltaTime;
            stateMachine.bulletRate = Mathf.Clamp(stateMachine.bulletRate, 0, stateMachine.bulletRateMax);
            if (fireCountdown <= 0)
            {
                if(!TryGetDirectionToTarget(stateMachine.superTarget, out Vector3 dir)){
                    return;
                }
                stateMachine.shooter.Fire(stateMachine.superBullet, superAttackInfo , dir);
                fireCountdown = 1 / stateMachine.bulletRate;
                Debug.Log(stateMachine.bulletRate);
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    public override void Exit()
    {
        stateMachine.action = "Null";
        stateMachine.bulletRate = 0;
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
