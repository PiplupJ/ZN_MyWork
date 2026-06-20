using UnityEngine;

public class ZenithThirdMeleeAttackState : ZenithAttackingState 
{
    public ZenithThirdMeleeAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private bool firstAttack;
    private bool secondAttack;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(MeleeAttackHash3, TransitionDuration);
        firstAttack = false;
        secondAttack = false;
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = GetNormalizedTime(stateMachine.Animator, "Attack_m3");
        //hitbox Onにする
        if(!firstAttack&&elapsedTime>=0.2f){

            stateMachine.sem.playEnemySE(EnemySEtype.ZenithMeleeAttack);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingL1);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingL2);
            firstAttack = true;
        }
        else if(!secondAttack&&elapsedTime>=0.4f){

            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingL1);
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingL2);

            stateMachine.sem.playEnemySE(EnemySEtype.ZenithMeleeAttack);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingR1);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingR2);
            
            secondAttack = true;
        }
        else if(elapsedTime>=0.6f)
        {
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingR1);
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingR2);
        }
            
            if(elapsedTime>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            //stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.CoolManager.MeleeAttackCoolDownOn();
    }
}