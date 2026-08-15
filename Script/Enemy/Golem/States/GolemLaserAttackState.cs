using UnityEngine;

public class GolemLaserAttackState : GolemBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Laser");

    private const float TransitionDuration = 0.1f;

    public GolemLaserAttackState(GolemStateMachine stateMachine) : base(stateMachine) { }
   
    enum LaserState
    {
        Ready, Fire, Recover, Finish
    }

    LaserState state;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        state = LaserState.Ready;
    }


    public override void Tick(float deltaTime)
    {   

        float elapsed = GetNormalizedTime(stateMachine.Animator, "Laser");

        switch(state)
        {
            case LaserState.Ready :
                if(elapsed >=0.1f){
                    stateMachine.hitboxController.HitBoxEnable(GolemAttackType.Laser);
                    SoundPlayer.Instance.PlaySE("G_LaserShot");
                    state = LaserState.Fire;
                }
                break;
            case LaserState.Fire :
                if(elapsed>=0.3f){
                    stateMachine.hitboxController.HitBoxDisable(GolemAttackType.Laser);
                    state = LaserState.Recover;
                }
                break;
            case LaserState.Recover :
                if(elapsed >=1){
                    state = LaserState.Finish;
                    stateMachine.SwitchState(new GolemChasingState(stateMachine));
                }
                break;
            default :
                break;
            
        }
    }

    public override void Exit()
    {
        stateMachine.hitboxController.HitBoxDisable(GolemAttackType.Laser);
        stateMachine.coolManager.LaserAttackCoolDownOn();
    }
}
