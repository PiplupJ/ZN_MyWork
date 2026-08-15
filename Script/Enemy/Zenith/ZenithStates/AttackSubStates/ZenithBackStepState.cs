using UnityEngine;

public class ZenithBackStepState : ZenithBaseState
{
    public ZenithBackStepState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    Vector3 moveDir;

    public override void Enter()
    {
        SoundPlayer.Instance.PlaySE("Z_BackStep");
        stateMachine.mAnimator.BackStep();
        if(TryGetDirToPlayer(out var dir)){
            moveDir = -dir;
        }
        else{
            moveDir = Vector3.zero;
        }
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Attack");
        
        RotateToPlayer(deltaTime);
        
        MoveWithFixedSpeed(deltaTime, stateMachine.backStepSpeed, moveDir);
        

        if(elapsedTime>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.currentVelocity = Vector3.zero;
    }
}
