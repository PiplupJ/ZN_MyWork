using UnityEngine;

public class ZenithBeamAttackState : ZenithAttackingState
{
    public ZenithBeamAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private float BeamAttackTime = 3.0f;
    private bool fire;
    private float timer;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BeamAttackHash1, TransitionDuration);
        stateMachine.sem.playEnemySE(EnemySEtype.ZenithBeamReady);
        fire = false;
        stateMachine.CoolManager.BeamAttackCoolDownOn();
        
    }

    public override void Tick(float deltaTime)
    {   
        timer = GetNormalizedTime(stateMachine.Animator, "Attack_Beam_Start");
        
        if(fire == false && timer>=1){
            stateMachine.sem.playEnemySE(EnemySEtype.ZenithBeamShot);
            stateMachine.hitManager.HitBoxEnable(ZenithHitBoxes.Beam);
            fire = true;
        }

        if(fire == true)
        {
            BeamAttackTime -= deltaTime;
        }

        if(BeamAttackTime<=0)
        {
            stateMachine.hitManager.HitBoxDisable(ZenithHitBoxes.Beam);
            stateMachine.SwitchState(new ZenithBeamFinishState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
