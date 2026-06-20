using UnityEngine;

public class ZenithWingFinishState : ZenithAttackingState
{
    public ZenithWingFinishState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(WingAttackHash2, TransitionDuration);

        stateMachine.sem.playEnemySE(EnemySEtype.ZenithWingFinish);
        stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingAttack);
    }
    
    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTime(stateMachine.Animator, "Attack_Wing_Finish")>=1){
		    stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingAttack);
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
