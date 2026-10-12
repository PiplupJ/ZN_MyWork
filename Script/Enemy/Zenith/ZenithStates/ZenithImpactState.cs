using UnityEngine;

public class ZenithImpactState : ZenithBaseState
{
    public ZenithImpactState(ZenithStateMachine stateMachine, KnockbackEntry entry) : base(stateMachine)
    {
        this.entry = entry;
    }

    KnockbackEntry entry;
    Vector3 pushDir;
    float speed;
    public override void Enter()
    {
        stateMachine.mAnimator.Impact();
        if(TryGetDirToPlayer(out pushDir))
        {
            pushDir = -pushDir;
        }
        speed = entry.initialSpeed;
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Impact");

        if(elapsedTime>=1.0f){
            stateMachine.SwitchState(new ZenithIdleState(stateMachine));
            return;
        }
    }

    public override void FixedTick(float deltaTime)
    {
        Quaternion targetRot = Quaternion.LookRotation(-pushDir);
        stateMachine.transform.rotation = Quaternion.RotateTowards(
            stateMachine.transform.rotation,
            targetRot,
            stateMachine.rotateSpeed * deltaTime);

        MoveWithFixedSpeed(deltaTime, speed, pushDir);

        speed = Mathf.Max(0, speed - entry.deceleration*deltaTime);        
    }

    public override void Exit()
    {

    }
}
