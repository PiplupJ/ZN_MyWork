using UnityEngine;

public class ZenithChasingState : ZenithBaseState
{
    private readonly int BasicMotionHash = Animator.StringToHash("BasicMotion");

    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private float DashChance = 0.5f;

    public ZenithChasingState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("ZenithChasingState"+Time.time);
        stateMachine.PrevState = ZenithStates.Chasing;
        stateMachine.Animator.CrossFadeInFixedTime(BasicMotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {   
        RotateToPlayer(deltaTime);
        
        if(stateMachine.CoolManager.CanAttack()&&stateMachine.CoolManager.CanDash())
        {
            float randomValue = Random.value;
            
            Debug.Log("chance was:"+randomValue);
            if(randomValue < DashChance)
            {
                stateMachine.SwitchState(new ZenithAttackingState(stateMachine));
                return;
            }
            else{
                stateMachine.SwitchState(new ZenithDashStartState(stateMachine));
                return;
            }
        }
        else if(stateMachine.CoolManager.CanAttack()==true)
        {
            stateMachine.SwitchState(new ZenithAttackingState(stateMachine));
            return;
        }
        else if(InRangedAttackRange()==false && stateMachine.CoolManager.CanDash()==true){
                stateMachine.SwitchState(new ZenithDashStartState(stateMachine));
                return;
            }
        else if(InChaseRange())
        {
            stateMachine.Animator.SetFloat(SpeedHash, 1.0f, AnimatorDampTime, deltaTime);
            if(InEscapeRange()==true)
            {
                EscapeFromPlayer(deltaTime, stateMachine.moveSpeed);
            }
            else
            {   
                MoveToPlayer(deltaTime, stateMachine.moveSpeed);
            }
        }
        else{
            stateMachine.SwitchState(new ZenithIdleState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {

    }
}
