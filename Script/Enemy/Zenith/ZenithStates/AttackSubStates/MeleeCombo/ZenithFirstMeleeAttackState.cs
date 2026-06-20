using UnityEngine;

public class ZenithFirstMeleeAttackState : ZenithAttackingState 
{
    public ZenithFirstMeleeAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    bool attacked;

    public override void Enter()
    {
        attacked = false;
        stateMachine.Animator.CrossFadeInFixedTime(MeleeAttackHash1, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = GetNormalizedTime(stateMachine.Animator, "Attack_m1");
        if(!attacked&&elapsedTime>0.25f)
        {
            stateMachine.sem.playEnemySE(EnemySEtype.ZenithMeleeAttack);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingL1);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingL2);
            attacked = true;
        }
        else if(attacked&&elapsedTime>0.6f)
        {
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingL1);
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingL2);
        }

        if(elapsedTime>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            //stateMachine.SwitchState(new ZenithSecondMeleeAttackState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.CoolManager.MeleeAttackCoolDownOn();
    }
}