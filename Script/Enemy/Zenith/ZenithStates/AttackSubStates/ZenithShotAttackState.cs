using System.Runtime.CompilerServices;
using UnityEngine;

public class ZenithShotAttackState : ZenithBaseState
{
    public ZenithShotAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private readonly float[] shotTimes = {0.46f, 0.48f, 0.5f, 0.52f};

    private readonly float[] shotAngles = { -8.0f, -4.0f, 4.0f, 8.0f }; //放射形発射時使う

    int shotIndex;
    AttackInfo attackInfo;

    private const float AIM_DISTANCE = 50.0f;

    public override void Enter()
    {
        stateMachine.mAnimator.Shot();
        shotIndex = 0;
        attackInfo = stateMachine.shotAttackData.GetAttackInfo(stateMachine.gameObject);
        if(TryGetDirToPlayer(out Vector3 dir))
        {
            stateMachine.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Attack");

        if(shotIndex < shotTimes.Length){
            if(elapsedTime >= shotTimes[shotIndex]){
                FireShot(stateMachine.FirePoints[shotIndex].transform.position);
                shotIndex++;
            }
        }

        if(elapsedTime >= 1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }        
    }

    private void FireShot(Vector3 firepoint)
    {
        Vector3 forward = stateMachine.transform.forward;
        Vector3 dir = forward;
        switch (stateMachine.phase)
        {
            case ZenithStateMachine.ZenithPhase.Phase1:
                
                break;
            case ZenithStateMachine.ZenithPhase.Phase2:
                Vector3 axis = stateMachine.transform.up;
                dir = Quaternion.AngleAxis(shotAngles[shotIndex], axis) * forward;
                break;
            default:
                break;
        }

        Vector3 targetPos = firepoint + dir * AIM_DISTANCE;
        SoundPlayer.Instance.PlaySE("Z_Shot");
        stateMachine.shooter.Fire(stateMachine.shotBullet, firepoint, attackInfo, targetPos);
    }

    public override void Exit()
    {

    }
}
