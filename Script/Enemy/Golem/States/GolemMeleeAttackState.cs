using UnityEngine;

public class GolemMeleeAttackState : GolemBaseState
{
    private readonly int AttackHash = Animator.StringToHash("NormalAttack");

    private const float TransitionDuration = 0.1f;

    public GolemMeleeAttackState(GolemStateMachine stateMachine) : base(stateMachine) { }

    private bool attacked; 
    private bool attackFinished;
    private float attackTime = 0.72f;
    private float attackFinTime = 0.8f;

    enum MeleeState
    {
        Ready, Attack, Recover
    }

    MeleeState state;

     public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        state = MeleeState.Ready;
        stateMachine.sem.playEnemySE(EnemySEtype.GolemMeleeReady);
    }

    public override void Tick(float deltaTime)
    {   
        float elapsed = GetNormalizedTime(stateMachine.Animator, "NormalAttack");
        
        switch(state)
        {
            case MeleeState.Ready:
                if(elapsed >= attackTime){
                    stateMachine.hitManager.HitBoxEnable(GolemAttackType.Melee);
                    stateMachine.sem.playEnemySE(EnemySEtype.GolemMeleeAttack);
                    state = MeleeState.Attack;
                }
                break;
            case MeleeState.Attack :
                if(elapsed>=attackFinTime)
                {
                    Vector3 playerPos = stateMachine.Player.transform.position;
                    Vector3 attackPoint = stateMachine.NA_target.transform.position;
                    float dist = Vector3.Distance(attackPoint, playerPos);
                    if(dist<=4.0f){
                        PlayerHP.instance.DealDamage(stateMachine.GetAttackPower());
                    }
                    state = MeleeState.Recover;
                }
                break;
            case MeleeState.Recover :
                if(elapsed>=1.0f){
                    stateMachine.SwitchState(new GolemChasingState(stateMachine));
                    return;
                }
                break;
            default :
                break;
        }
    }

    public override void Exit()
    {
        stateMachine.coolManager.MeleeAttackCoolDownOn();
    }
}
