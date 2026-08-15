using UnityEngine;

public class ZenithWingAttackState : ZenithBaseState
{
    public ZenithWingAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }
    
    enum AttackPhase
    {
        Ready,
        Attack,
        Recover,
        Finish
    }
    AttackPhase phase;
    
    public override void Enter()
    {
        phase = AttackPhase.Ready;
        stateMachine.mAnimator.Wing();
    }

    public override void Tick(float deltaTime)
    {   
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Attack");

        switch(phase)
        {
            case AttackPhase.Ready :
                if(elapsedTime >= 0.7f){
                    SoundPlayer.Instance.PlaySE("Z_Wing");
                    stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.LeftWing);
                    stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.RightWing); 
                    phase = AttackPhase.Attack;
                }
                break;
            case AttackPhase.Attack :
                if(elapsedTime >= 0.9f){
                    stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.LeftWing);
                    stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.RightWing);
                    phase = AttackPhase.Recover;
                }
                break;
            case AttackPhase.Recover :
                if(elapsedTime >= 1.0f){
                    stateMachine.SwitchState(new ZenithWingFinishState(stateMachine));
                    phase = AttackPhase.Finish;
                }
                break;
            default :
                break;
        }
    }

    public override void Exit()
    {
        stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.LeftWing);
        stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.RightWing);
    }
}
