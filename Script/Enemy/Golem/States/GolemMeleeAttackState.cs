using UnityEngine;

public class GolemMeleeAttackState : GolemPreMeleeAttackState
{
    private readonly int AttackHash = Animator.StringToHash("NormalAttack");

    private const float TransitionDuration = 0.1f;

    public GolemMeleeAttackState(GolemStateMachine stateMachine) : base(stateMachine) { }

    private bool attacked; 
    private bool attackFinished;
    private float attackTime = 0.72f;
    private float attackFinTime = 0.8f;

     public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        attacked = false;
        attackFinished = false;
        stateMachine.sem.playEnemySE(EnemySEtype.GolemMeleeReady);
    }

    public override void Tick(float deltaTime)
    {   

        if(attacked==false && GetNormalizedTime(stateMachine.Animator, "NormalAttack")>=attackTime){
            attacked = true;
            stateMachine.hitManager.HitBoxEnable(GolemAttackType.Melee);
            stateMachine.sem.playEnemySE(EnemySEtype.GolemMeleeAttack);
            Debug.Log("MeleeAttack!");
        }
        else if(attackFinished == false && GetNormalizedTime(stateMachine.Animator, "NormalAttack")>=attackFinTime){
            Vector3 playerPos = stateMachine.Player.transform.position;
            Vector3 attackPoint = stateMachine.NA_target.transform.position;
            float dist = Vector3.Distance(attackPoint, playerPos);
            if(dist<=4.0f){
                PlayerHP.instance.DealDamage(stateMachine.GetAttackPower());
            }
            //stateMachine.hitManager.HitBoxDisable(GolemAttackType.Melee);
            attackFinished = true;
        }
        else if(GetNormalizedTime(stateMachine.Animator, "NormalAttack")>=1){
            stateMachine.SwitchState(new GolemChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.coolManager.MeleeAttackCoolDownOn();
    }
}
