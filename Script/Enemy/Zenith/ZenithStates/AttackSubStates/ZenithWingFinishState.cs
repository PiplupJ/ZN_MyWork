using UnityEngine;

public class ZenithWingFinishState : ZenithBaseState
{
    public ZenithWingFinishState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.mAnimator.WingFinish();

        SoundPlayer.Instance.PlaySE("Z_WingFinish");
        stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.LeftWing);
        stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.RightWing); 
    }
    
    public override void Tick(float deltaTime)
    {
        if(stateMachine.mAnimator.GetNormalizedTime("Attack")>=1){
		    stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.LeftWing);
            stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.RightWing);
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.LeftWing);
        stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.RightWing);
    }
}
