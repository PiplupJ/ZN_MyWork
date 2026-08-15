using UnityEngine;

public class GolemDeadState : GolemBaseState
{
   public GolemDeadState(GolemStateMachine stateMachine) : base(stateMachine) { }

    private readonly int DeadHash = Animator.StringToHash("Dead");

    private const float CrossFadeDuration = 0.1f;

    public override void Enter()
    {
        SoundPlayer.Instance.PlaySE("G_Dead");
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
