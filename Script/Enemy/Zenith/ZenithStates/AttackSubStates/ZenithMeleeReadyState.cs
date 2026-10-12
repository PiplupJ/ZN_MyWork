using UnityEngine;

public class ZenithMeleeReadyState : ZenithBaseState
{
    public ZenithMeleeReadyState(ZenithStateMachine stateMachine, ZenithActionType nextAction) : base(stateMachine)
    {
        this.nextAction = nextAction;
    }

    ZenithActionType nextAction;

    public override void Enter()
    {
        stateMachine.health.StartSuperArmor();
        stateMachine.mAnimator.Ready();
        SoundPlayer.Instance.PlaySE(SoundType.ChargeSE);
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Ready");

        if(elapsedTime >= 1.0f)
        {
            SwitchToNextAction();
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.health.EndSuperArmor();
    }

    private void SwitchToNextAction()
    {
        switch(nextAction)
        {
            case ZenithActionType.Melee:
                stateMachine.SwitchState(new ZenithMeleeAttackState(stateMachine));
                break;
            case ZenithActionType.DoubleMelee:
                stateMachine.SwitchState(new ZenithDoubleMeleeAttackState(stateMachine));
                break;
            default :
                stateMachine.SwitchState(new ZenithIdleState(stateMachine));
                break;
        }
    }
}
