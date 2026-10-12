using UnityEngine;

public class ZenithSupernovaState : ZenithBaseState
{
    public ZenithSupernovaState(ZenithStateMachine stateMachine) : base(stateMachine){}

    enum AttackPhase
    {
        Start,
        End
    }
    AttackPhase phase;
    float standbyTimer = 3.0f;
    public override void Enter()
    {
        stateMachine.health.StartSuperArmor();
        stateMachine.mAnimator.SupernovaStart();
        phase = AttackPhase.Start;
        SoundPlayer.Instance.PlaySE(SoundType.ChargeSE);
    }

    public override void Tick(float deltaTime)
    {
        switch(phase)
        {
            case AttackPhase.Start:
                float t = stateMachine.mAnimator.GetNormalizedTime("Supernova");
                if (t>=1)
                {
                    SupernovaAttack attack = Object.Instantiate(
                            stateMachine.supernova,
                            stateMachine.transform.position,
                            Quaternion.identity
                        );
                    attack.SetAttack(stateMachine.supernovaAttackData.GetAttackInfo(stateMachine.gameObject));
                    SoundPlayer.Instance.PlaySE(SoundType.SupernovaSE);
                    stateMachine.mAnimator.SupernovaEnd();
                    phase = AttackPhase.End;
                }
                break;
            case AttackPhase.End:
                standbyTimer -= deltaTime;
                if (standbyTimer<=0)
                {
                    stateMachine.SwitchState(new ZenithIdleState(stateMachine));
                }
                break;
            default:
                break;
        }
    }

    public override void Exit()
    {
        stateMachine.health.EndSuperArmor();
    }
}
