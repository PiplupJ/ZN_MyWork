using UnityEngine;

public class GolemMeleeAttackState : GolemBaseState
{
    private readonly int AttackHash = Animator.StringToHash("NormalAttack");

    private const float TransitionDuration = 0.03f;

    public GolemMeleeAttackState(GolemStateMachine stateMachine) : base(stateMachine) { }

    private float attackTime = 0.78f;
    private float attackFinTime = 0.8f;

    enum AttackPhase
    {
        Ready, Attack, Recover, Finish
    }

    AttackPhase phase;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        phase = AttackPhase.Ready;
        SoundPlayer.Instance.PlaySE("G_MeleeReady");
    }

    public override void Tick(float deltaTime)
    {   
        float elapsed = GetNormalizedTime(stateMachine.Animator, "NormalAttack");
        
        switch(phase)
        {
            case AttackPhase.Ready:
                if(elapsed >= attackTime){
                    
                    SoundPlayer.Instance.PlaySE("G_MeleeAttack");
                    stateMachine.hitboxController.HitBoxEnable(GolemAttackType.Melee);
                    phase = AttackPhase.Attack;
                }
                break;
            case AttackPhase.Attack :
                if(elapsed>=attackFinTime)
                {
                    phase = AttackPhase.Recover;
                    stateMachine.hitboxController.HitBoxDisable(GolemAttackType.Melee);
                }
                break;
            case AttackPhase.Recover :
                if(elapsed>=1.0f){
                    stateMachine.SwitchState(new GolemChasingState(stateMachine));
                    phase = AttackPhase.Finish;
                }
                break;
            default :
                break;
        }
    }

    public override void Exit()
    {
        stateMachine.hitboxController.HitBoxDisable(GolemAttackType.Melee);
        stateMachine.coolManager.MeleeAttackCoolDownOn();
    }
}
