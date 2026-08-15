using UnityEngine;

public class ZenithShotAttackState : ZenithBaseState
{
    public ZenithShotAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private readonly float[] shotTimes = {0.46f, 0.48f, 0.5f, 0.52f};
    
    int shotIndex;
    AttackInfo attackInfo;

    public override void Enter()
    {
        stateMachine.mAnimator.Shot();
        shotIndex = 0;
        attackInfo = stateMachine.shotAttackData.GetAttackInfo(stateMachine.gameObject);
    }

    public override void Tick(float deltaTime)
    {
        float elapsedTime = stateMachine.mAnimator.GetNormalizedTime("Attack");

        if(shotIndex < shotTimes.Length){
            if(elapsedTime >= shotTimes[shotIndex]){
                SoundPlayer.Instance.PlaySE("Z_Shot");
                stateMachine.shooter.Fire(stateMachine.shotBullet, stateMachine.FirePoints[shotIndex].transform.position, attackInfo, stateMachine.transform.forward);
                shotIndex++;
            }
        }

        if(elapsedTime >= 1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }        
    }

    public override void Exit()
    {

    }
}
