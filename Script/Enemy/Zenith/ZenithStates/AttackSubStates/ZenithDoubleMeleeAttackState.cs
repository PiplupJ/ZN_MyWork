using UnityEngine;

public class ZenithDoubleMeleeAttackState : ZenithBaseState
{
    public ZenithDoubleMeleeAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    enum AttackPhase
    {
        WindUp,
        Active,
        Wait,
        Chain,
        Recover,
        Finish
    }
    AttackPhase phase;

    public override void Enter()
    {
        phase = AttackPhase.WindUp;
        stateMachine.mAnimator.DoubleMelee();
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Attack");

        switch(phase)
        {
            case AttackPhase.WindUp :
                if(elapsedTime>=0.2f){
                    SoundPlayer.Instance.PlaySE("Z_Melee", Random.Range(-0.2f, 0.2f));
                    stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.LeftWing);    
                    phase = AttackPhase.Active;              
                }  
            break;
            case AttackPhase.Active :
                if(elapsedTime>=0.3f){

                    stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.LeftWing);               
                    phase = AttackPhase.Wait;
                }
                break;
            case AttackPhase.Wait :
                if(elapsedTime>=0.4f){
                    SoundPlayer.Instance.PlaySE("Z_Melee", Random.Range(-0.2f, 0.2f));
                    stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.RightWing);                    
                    phase = AttackPhase.Chain;
                }
                break;
            case AttackPhase.Chain :
                if(elapsedTime>=0.6f){
                    stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.RightWing);                  
                    phase = AttackPhase.Recover;                    
                }
                break;
            case AttackPhase.Recover :
                if(elapsedTime >= 1){
                    phase = AttackPhase.Finish;
                }
                break;
            case AttackPhase.Finish :
                stateMachine.SwitchState(new ZenithChasingState(stateMachine));
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
