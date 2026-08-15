using UnityEngine;

public class ZenithBeamAttackState : ZenithBaseState
{
    public ZenithBeamAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private float BeamAttackTime = 3.0f;
    private bool fire;
    private float timer;

    public override void Enter()
    {
        SoundPlayer.Instance.PlaySE("Z_BeamCharge");
        stateMachine.mAnimator.Beam();
        fire = false;
    }

    public override void Tick(float deltaTime)
    {   
        timer = stateMachine.mAnimator.GetNormalizedTime("Attack");
        
        if(fire == false && timer>=1){
            SoundPlayer.Instance.PlaySE("Z_Beam");
            stateMachine.hitboxController.ActivateHitbox(ZenithHitboxType.Laser);
            fire = true;
        }

        if(fire == true)
        {
            BeamAttackTime -= deltaTime;
        }

        if(BeamAttackTime<=0)
        {
            stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.Laser);
            stateMachine.SwitchState(new ZenithBeamFinishState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.hitboxController.DeactivateHitbox(ZenithHitboxType.Laser);
    }
}
