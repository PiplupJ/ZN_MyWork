using UnityEngine;

public class ZenithSecondMeleeAttackState : ZenithAttackingState 
{
    public ZenithSecondMeleeAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    bool attacked;

    public override void Enter()
    {
        attacked = false;
        stateMachine.Animator.CrossFadeInFixedTime(MeleeAttackHash2, TransitionDuration);
        
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = GetNormalizedTime(stateMachine.Animator, "Attack_m2");

         if(!attacked&&elapsedTime>0.25f)
        {
            stateMachine.sem.playEnemySE(EnemySEtype.ZenithMeleeAttack);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingR1);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingR2);
            attacked = true;
        }
        else if(attacked&&elapsedTime>0.6f)
        {
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingR1);
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingR2);
        }

        if(GetNormalizedTime(stateMachine.Animator, "Attack_m2")>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            //stateMachine.SwitchState(new ZenithThirdMeleeAttackState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.CoolManager.MeleeAttackCoolDownOn();
    }
}