using UnityEngine;

public class ZenithMeleeAttackState : ZenithBaseState
{
    public ZenithMeleeAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    enum AttackSide
    {
        Right,
        Left
    }
    AttackSide attackSide;

    enum AttackPhase
    {
        WindUp,
        Active,
        Recover,
        Finish
    }
    AttackPhase phase;
    

    public override void Enter()
    {

        if(Random.value < 0.5f){
            attackSide = AttackSide.Right;
        }
        else{
            attackSide = AttackSide.Left;
        }

        phase = AttackPhase.WindUp;

        if(attackSide == AttackSide.Right){
            stateMachine.mAnimator.MeleeRight();
        }
        else{
            stateMachine.mAnimator.MeleeLeft();
        }
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Attack");

        switch(phase)
        {
            case AttackPhase.WindUp :
                if(elapsedTime>=0.25f){
                    if(attackSide==AttackSide.Right){
                        stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.RightWing);   
                    }
                    else{
                        stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.LeftWing);                    
                    }
                    phase = AttackPhase.Active;
                }    

            break;
            case AttackPhase.Active :
                if(elapsedTime>=0.5f){
                    if(attackSide==AttackSide.Right){
                        stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.RightWing);   
                    }
                    else{
                        stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.LeftWing);                   
                    }
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
