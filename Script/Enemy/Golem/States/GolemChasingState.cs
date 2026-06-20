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
        Debug.Log("Enter GolemChasingState");
        stateMachine.Animator.applyRootMotion = true;
    }

    public override void Tick(float deltaTime)
    {   


        if(OnSight())
        {
            if(!InLaserAttackRange())
            {
                if(stateMachine.coolManager.CanRockfallAttack()){
                    stateMachine.SwitchState(new GolemRockfallAttackState(stateMachine));
                    return;
                }
                else{
                    GolemChase(deltaTime);
                }
            }
            else{
                if(stateMachine.coolManager.CanLaserAttack()){
                    stateMachine.SwitchState(new GolemPreLaserAttackState(stateMachine));
                    return;
                }
                else if(stateMachine.coolManager.CanMeleeAttack()){
                    stateMachine.SwitchState(new GolemMeleeAttackState(stateMachine));
                    return;
                }
                else{
                    GolemChase(deltaTime);
                }
            }   
        }
        else{
            GolemChase(deltaTime);
        }

        /*
        if(!InChaseRange())
        {
            stateMachine.SwitchState(new GolemIdleState(stateMachine));
            return;
        }
        if(InMeleeAttackRange())
        {
            if(stateMachine.coolManager.CanMeleeAttack())
            {
                stateMachine.SwitchState(new GolemMeleeAttackState(stateMachine));
                return;
            }
            else
            {
                stateMachine.SwitchState(new GolemPreLaserAttackState(stateMachine));
                return;
            }
        }
        else if(InLaserAttackRange())
        {
            if(stateMachine.coolManager.CanLaserAttack())
            {
                stateMachine.SwitchState(new GolemPreLaserAttackState(stateMachine));
                return;
            }
            else if(stateMachine.coolManager.CanRockfallAttack())
            {
                stateMachine.SwitchState(new GolemRockfallAttackState(stateMachine));
                return;
            }
            else {
                    GolemChase(true, deltaTime);
                }    
        }
        else{
            if(stateMachine.coolManager.CanRockfallAttack())
            {
                stateMachine.SwitchState(new GolemRockfallAttackState(stateMachine));
                return;
            }
            else {
                GolemChase(true, deltaTime);
            }
        }
        */
    }

    public override void Exit()
    {

    }

    private void GolemChase(float deltaTime)
    {
        if(stateMachine.Player == null) { return; }

        Vector3 goal = stateMachine.Player.transform.position;
        Vector3 moveDir = (goal - stateMachine.GolemController.transform.position).normalized;
                
        stateMachine.Animator.SetFloat(SpeedXHash, moveDir.x, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(SpeedYHash, moveDir.z, AnimatorDampTime, deltaTime);

        if(InMeleeAttackRange()){
            MoveAndRotateToPlayer(deltaTime, 0);
        }
        else{
            MoveAndRotateToPlayer(deltaTime, stateMachine.moveSpeed);
        }
        
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
