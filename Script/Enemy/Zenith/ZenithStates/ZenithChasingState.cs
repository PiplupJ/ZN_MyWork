using UnityEngine;

public class ZenithChasingState : ZenithBaseState
{

    public ZenithChasingState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private int strafeDir = 1;
    private float strafeFlipTimer;
    private float flipInterval = 3.0f;
    private float strafeSpeedRate = 0.4f;

    public override void Enter()
    {
        stateMachine.mAnimator.Idle();
        stateMachine.mAnimator.SetBlendMotion(1.0f);

        strafeFlipTimer = flipInterval;
    }

    public override void Tick(float deltaTime)
    {   

        RotateToPlayer(deltaTime);

        if(OnSight()&&stateMachine.brain.CanDecide()){
            ZenithActionType type = stateMachine.brain.DecideAction(DistToPlayer(), stateMachine.health.GetHealthRatio());

            if(type!=ZenithActionType.None&&TryAction(type))
            {
                return;
            }

            stateMachine.brain.ScheduleNextDecision();
        }

        MaintainDistance(deltaTime);
        
        if(!InChaseRange()){
            stateMachine.SwitchState(new ZenithIdleState(stateMachine));
            return;
        }
    }

    //行動切り替え
    private bool TryAction(ZenithActionType type)
    {
        switch(type)
        {
            case ZenithActionType.Dash :
                stateMachine.SwitchState(new ZenithDashStartState(stateMachine));
                return true;
            case ZenithActionType.BackStep :
                stateMachine.SwitchState(new ZenithBackStepState(stateMachine));
                return true;
            case ZenithActionType.Melee :
                stateMachine.SwitchState(new ZenithMeleeAttackState(stateMachine));
                return true;
            case ZenithActionType.DoubleMelee :
                stateMachine.SwitchState(new ZenithDoubleMeleeAttackState(stateMachine));
                return true;
            case ZenithActionType.Laser :
                stateMachine.SwitchState(new ZenithBeamAttackState(stateMachine));
                return true;
            case ZenithActionType.Wing :
                stateMachine.SwitchState(new ZenithWingAttackState(stateMachine));
                return true;
            case ZenithActionType.Shot :
                stateMachine.SwitchState(new ZenithShotAttackState(stateMachine));
                return true;
            default :
                return false;
        }
    }
    //距離維持
    private void MaintainDistance(float deltaTime)
    {
        //float safeDistance = stateMachine.targetRange*1.5f;

        //if(InTargetRange()){
          //  if(TryGetDirToPlayer(out var dir)){
            //    MoveWithAccel(deltaTime, stateMachine.chaseSpeed, -dir);
            //}
        //}
        //else 
        if(DistToPlayer() < stateMachine.targetRange){
            //Strafe(deltaTime);
            stateMachine.mAnimator.SetBlendMotion(0);
        }
        else{
            if(TryGetDirToPlayer(out var dir)){
                MoveWithAccel(deltaTime, stateMachine.chaseSpeed, dir);
                stateMachine.mAnimator.SetBlendMotion(1.0f);
            }
        }
    }
    //プレイヤーの周辺を回す
    private void Strafe(float deltaTime)
    {
        if(!TryGetDirToPlayer(out Vector3 dir)){
            return;
        }

        strafeFlipTimer -= deltaTime;
        if(strafeFlipTimer <=0){
            if (Random.value < 0.3f) { strafeDir = -strafeDir; }
            strafeFlipTimer = flipInterval*Random.Range(0.7f, 1.3f);
        }

        Vector3 sideDir = Vector3.Cross(Vector3.up, dir) * strafeDir;

        MoveWithAccel(deltaTime, stateMachine.chaseSpeed*strafeSpeedRate, sideDir);
    }

    public override void Exit()
    {

    }
}
