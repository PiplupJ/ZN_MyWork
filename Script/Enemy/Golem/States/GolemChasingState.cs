using UnityEngine;

public class GolemChasingState : GolemBaseState
{
    private readonly int BasicMotionHash = Animator.StringToHash("BasicMotion");

    private readonly int SpeedXHash = Animator.StringToHash("SpeedX");
    private readonly int SpeedYHash = Animator.StringToHash("SpeedY");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    float attackAngle = 15.0f;

    public GolemChasingState(GolemStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BasicMotionHash, CrossFadeDuration);
        //Debug.Log("Enter GolemChasingState");
        //stateMachine.Animator.applyRootMotion = true;
    }

    public override void Tick(float deltaTime)
    {   
        if(TryStartAttack()) return;

        UpdateMoveAnimation(deltaTime);
    }

    public override void FixedTick(float deltaTime)
    {
        float speed = InMeleeAttackRange() ? 0f : stateMachine.moveSpeed;

        if(TryGetDirToPlayer(out Vector3 dir))
        {
            MoveAndRotateToPlayer(deltaTime, speed);
        }
    }

    public override void Exit()
    {

    }

    private void UpdateMoveAnimation(float deltaTime)
    {
        if(!TryGetDirToPlayer(out Vector3 dir))
        {
            return;
        }

        Vector3 moveDir = dir.normalized;
                
        stateMachine.Animator.SetFloat(SpeedXHash, moveDir.x, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(SpeedYHash, moveDir.z, AnimatorDampTime, deltaTime);        
    }

    bool TryStartAttack()
    {
        if(!OnSight()){ return false;}

            if(!InLaserAttackRange())
            {
                if(stateMachine.coolManager.CanRockfallAttack()){
                    stateMachine.SwitchState(new GolemRockfallAttackState(stateMachine));
                    return true;
                }
                return false;
            }

        if (stateMachine.coolManager.CanLaserAttack())
        {
            stateMachine.SwitchState(new GolemPreLaserAttackState(stateMachine));
            return true;
        }
        if (stateMachine.coolManager.CanMeleeAttack())
        {
            stateMachine.SwitchState(new GolemMeleeAttackState(stateMachine));
            return true;
        }
        return false;
    }

    bool OnSight()
    {
        if(stateMachine.Player == null) { return false; }

        Vector3 dir = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        dir.y = 0;
        Vector3 fwd = stateMachine.transform.forward;
        fwd.y = 0;

        float angle = Vector3.Angle(fwd, dir);

        return angle < attackAngle;
    }
}
