using UnityEngine;

public class NadirSuperCutInState : NadirBaseState
{
    
    public NadirSuperCutInState(NadirStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (stateMachine.superTarget == null)
        {
            stateMachine.superTarget = GameObject.FindGameObjectWithTag("SuperAttackTarget");
        }
        
        CameraController.Instance.SaveCurrentMode();
        CameraController.Instance.SetSuperCutIn();
        PlayerHP.instance.SetMuteki();
        stateMachine.mAnimator.SuperCutIn();
        stateMachine.ultimateKit.PlayEffects();
        SoundPlayer.Instance.PlaySE(SoundType.UltimateStartSE);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.superTarget == null)
        {
            stateMachine.ResetSuperGauge();
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
            return;
        }

        if (stateMachine.mAnimator.GetNormalizedTime("Super") >= 1)
        {
            stateMachine.SwitchState(new NadirSuperStrikeState(stateMachine));
        }
    }

    public override void FixedTick(float deltaTime)
    {
        RotateToBoss();
    }

    public override void Exit()
    {
        CameraController.Instance.SetSuperAttack();
        stateMachine.ultimateKit.StopEffects();
    }
}
