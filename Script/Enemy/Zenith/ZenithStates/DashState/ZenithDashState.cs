using UnityEngine;

public class ZenithDashState : ZenithBaseState
{
    public ZenithDashState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private Vector3 targetPos;
    private Vector3 dashDir;
    private float dashTimer;

    private const float StopRange = 1f; //止まる距離
    private const float DashDuration = 3f; //最大ダッシュ時間

    public override void Enter()
    {
        if (!TryGetPlayerPos(out targetPos))   
        {
            targetPos = stateMachine.transform.position
                      + stateMachine.transform.forward * 10f;
        }
        targetPos.y = stateMachine.transform.position.y;

        Vector3 diff = targetPos - stateMachine.transform.position;
        
        if(diff.sqrMagnitude > 0){
            dashDir = diff.normalized;
        }
        else{
            dashDir = stateMachine.transform.forward;
        }
        dashTimer = DashDuration;

        stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation, Quaternion.LookRotation(dashDir), 360);

        stateMachine.mAnimator.Dash();

    }

    public override void Tick(float deltaTime)
    {   
        dashTimer -= deltaTime;

        Vector3 toTarget = targetPos - stateMachine.transform.position;
        bool arrived = toTarget.sqrMagnitude <= StopRange || DistToPlayer() <= StopRange;
        bool passed   = Vector3.Dot(toTarget, dashDir) < 0f;
        bool timedOut = dashTimer <= 0f;
        
        if(arrived||passed||timedOut){
            stateMachine.SwitchState(new ZenithDashLandState(stateMachine));
            return;
        }

        MoveWithFixedSpeed(deltaTime, stateMachine.dashSpeed, dashDir);
    }

    public override void Exit()
    {
        stateMachine.currentVelocity = Vector3.zero;
    }
}
