using UnityEngine;
using UnityEngine.Assertions.Must;

public class NadirSuperStrikeState : NadirBaseState
{
    public NadirSuperStrikeState(NadirStateMachine stateMachine) : base(stateMachine) { }

    enum AttackPhase
    {
        WindUp,
        Active,
        Recover,
        Finish
    }
    AttackPhase phase;

    public override void Enter()
    {
        CameraController.Instance.SetSuperAttack();
        stateMachine.weapon.weaponFX.Begin();
        stateMachine.mAnimator.SuperStrike();
        GlobalHitStop.Instance.TriggerSlowMotion(0.5f);
        SoundPlayer.Instance.PlaySE(SoundType.UltimateSwingSE);
        phase = AttackPhase.WindUp;
    }

    public override void Tick(float deltaTime)
    {
        //スーパーストライク使用中は無敵
        PlayerHP.instance.SetMuteki();

        if (stateMachine.superTarget == null)
        {
            stateMachine.ResetSuperGauge();
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
            return;
        }

        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Super");

        switch (phase)
        {
            case AttackPhase.WindUp:
                if (elapsedTime >= stateMachine.ultimateKit.ultimateAttackdata.activeFrame)
                {
                    phase = AttackPhase.Active;
                    GlobalHitStop.Instance.EndSlowMotion();
                    FireSuperStrike();
                }
                break;
            case AttackPhase.Active:
                if(elapsedTime >= stateMachine.ultimateKit.ultimateAttackdata.recoverFrame)
                {
                    phase = AttackPhase.Recover;
                    stateMachine.weapon.weaponFX.End();
                }
                break;
            case AttackPhase.Recover:
                if (elapsedTime >= 1)
                {
                    phase = AttackPhase.Finish;
                    stateMachine.SwitchState(new NadirIdleState(stateMachine));
                }
                break;
            case AttackPhase.Finish:

                break;
            default:
                break;

        }

    }

    public override void FixedTick(float deltaTime)
    {
        
    }

    public override void Exit()
    {
        stateMachine.weapon.weaponFX.End();
        CameraController.Instance.ReturnToPrevMode();
        GlobalHitStop.Instance.EndSlowMotion();
        stateMachine.ResetSuperGauge();
    }

    private void FireSuperStrike()
    {
        if(stateMachine.superTarget==null) { return; }
        stateMachine.shooter.Fire(
            stateMachine.ultimateKit.ultimateProjectile, 
            stateMachine.ultimateKit.ultimateAttackdata.GetAttackInfo(stateMachine.gameObject), 
            stateMachine.superTarget.transform.position);
    }
}
