using UnityEngine;

public class ZenithWingAttackState : ZenithAttackingState
{
    public ZenithWingAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }
    
    private float transitionDelay;
    private bool throwed;
    private bool finished;
    
    public override void Enter()
    {
        transitionDelay = 1.0f;
        throwed = false;
        stateMachine.Animator.CrossFadeInFixedTime(WingAttackHash1, TransitionDuration);
        finished = false;
    }

    public override void Tick(float deltaTime)
    {   
        float elapsedTime = GetNormalizedTime(stateMachine.Animator, "Attack_Wing_Start");

        if(!throwed&&elapsedTime>=0.7f)
        {
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.WingAttack);
            throwed = true;
        }

        if(elapsedTime>=1)
        {   
            if(!finished)
            {
                stateMachine.sem.playEnemySE(EnemySEtype.ZenithWing);
                stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.WingAttack);
                finished = true;
            }
            transitionDelay-=deltaTime;
        }

        if(transitionDelay<=0){
            stateMachine.SwitchState(new ZenithWingFinishState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.CoolManager.WingAttackCoolDownOn();
    }
}
