using UnityEngine;

public class GolemDeadState : GolemBaseState
{
   public GolemDeadState(GolemStateMachine stateMachine) : base(stateMachine) { }

    private readonly int DeadHash = Animator.StringToHash("Dead");

    private const float CrossFadeDuration = 0.1f;

    public override void Enter()
    {
        stateMachine.sem.playEnemySE(EnemySEtype.GolemDeath);
       stateMachine.Animator.CrossFadeInFixedTime(DeadHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime) 
    {
        if(GetNormalizedTime(stateMachine.Animator, "Dead")>=1){
            stateMachine.DestroyGolem();
            return; 
        }
    }

    public override void Exit() { }
}
