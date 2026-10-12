using UnityEngine;

public class NadirImpactState : NadirBaseState
{
    public NadirImpactState(NadirStateMachine stateMachine, KnockbackEntry entry) : base(stateMachine)
    {
        this.entry = entry;
    }

    KnockbackEntry entry;
    Vector3 pushDir;
    float speed;
    float stuntime;
    public override void Enter()
    {
        stateMachine.mAnimator.Impact();
        var boss = BattleSceneManager.CurrentBoss;
        if(TryGetDirectionToTarget(boss.gameObject, out pushDir))
        {
            pushDir = -pushDir;
        }
        speed = entry.initialSpeed;
        stuntime = entry.cancelNormalizedTime;
    }

    public override void Tick(float deltaTime)
    {
        float t = stateMachine.mAnimator.GetNormalizedTime("Impact");
        stuntime -= deltaTime;

        if(t>=1.0f && stuntime <= 0){
            stateMachine.SwitchState(new NadirIdleState(stateMachine));
        }
    }

    public override void FixedTick(float deltaTime)
    {
        MoveWithFixedSpeed(speed, deltaTime, pushDir);
        speed = Mathf.Max(0, speed - entry.deceleration*deltaTime);
    }

    public override void Exit()
    {
        
    }
}
