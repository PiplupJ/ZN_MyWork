using UnityEngine;

public class GolemLaserAttackState : GolemBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Laser");

    private const float TransitionDuration = 0.1f;

    public GolemLaserAttackState(GolemStateMachine stateMachine) : base(stateMachine) { }
   
    private bool fire;
    private bool off;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        fire = false;
        off = false;
    }


    public override void Tick(float deltaTime)
    {   
        if(fire==false&&GetNormalizedTime(stateMachine.Animator, "Laser")>=0.1){
            Debug.Log("LaserOn!");
            stateMachine.hitManager.HitBoxEnable(GolemAttackType.Laser);
            stateMachine.sem.playEnemySE(EnemySEtype.GolemLaserShot);
            fire = true;
        }
        if(off==false && GetNormalizedTime(stateMachine.Animator, "Laser")>=0.3){
            stateMachine.hitManager.HitBoxDisable(GolemAttackType.Laser);
            off = true;
        }
        if(GetNormalizedTime(stateMachine.Animator, "Laser")>=1){
            stateMachine.coolManager.LaserAttackCoolDownOn();
            stateMachine.SwitchState(new GolemChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
