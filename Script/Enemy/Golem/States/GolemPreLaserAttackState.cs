using UnityEngine;

public class GolemPreLaserAttackState : GolemBaseState
{
    private readonly int AttackHash = Animator.StringToHash("LaserOn");

    private const float TransitionDuration = 0.1f;

    public GolemPreLaserAttackState(GolemStateMachine stateMachine) : base(stateMachine) { }
   

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
     
        stateMachine.sem.playEnemySE(EnemySEtype.GolemLaserReady);
    }


    public override void Tick(float deltaTime)
    {   
        //RotateToPlayer(deltaTime);

        if(GetNormalizedTime(stateMachine.Animator, "LaserOn")>=1){
            stateMachine.SwitchState(new GolemLaserAttackState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
