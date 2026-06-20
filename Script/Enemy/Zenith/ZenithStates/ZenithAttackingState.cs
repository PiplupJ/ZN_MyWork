using UnityEngine;

public class ZenithAttackingState : ZenithBaseState
{
    protected readonly int MeleeAttackHash1 = Animator.StringToHash("Attack_m1");
    protected readonly int MeleeAttackHash2 = Animator.StringToHash("Attack_m2");
    protected readonly int MeleeAttackHash3 = Animator.StringToHash("Attack_m3");

    protected readonly int ShotAttackHash = Animator.StringToHash("Attack_Shot");
    protected readonly int BeamAttackHash1 = Animator.StringToHash("Attack_Beam_Start");
    protected readonly int BeamAttackHash2 = Animator.StringToHash("Attack_Beam_Finish");

    protected readonly int WingAttackHash1 = Animator.StringToHash("Attack_Wing_Start");
    protected readonly int WingAttackHash2 = Animator.StringToHash("Attack_Wing_Finish");

    protected const float TransitionDuration = 0.1f;

    protected ZenithAttackType currentAttack;

    private float WingAttackRange = 6.0f;
    //private float BeamAttackChance = 0.5f;

    public ZenithAttackingState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("ZenithAttackingState"+Time.time);

        stateMachine.CoolManager.AttackCoolDownOn();
        
        if(stateMachine.PrevState == ZenithStates.Chasing){
            if(InMeleeAttackRange()&&stateMachine.CoolManager.CanMeleeAttack())
            {
                currentAttack = ZenithAttackType.Melee;
            }
            else if(InWingAttackRange()&&stateMachine.CoolManager.CanWingAttack())
            {
                currentAttack = ZenithAttackType.Wing; 
            }
            else if(stateMachine.CoolManager.CanBeamAttack()){
                currentAttack = ZenithAttackType.Beam;
            }
            else if(stateMachine.CoolManager.CanShotAttack())
            {
                currentAttack = ZenithAttackType.Shot;
            }
        }
        else if(stateMachine.PrevState == ZenithStates.Dash)
        {
            if(InMeleeAttackRange()==true)
            {
                currentAttack = ZenithAttackType.Melee;
            }
            else if(stateMachine.phase == ZenithPhase.Phase2){
                currentAttack = ZenithAttackType.Shot;
            }
        }
        stateMachine.PrevState = ZenithStates.Attacking;
        //stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        switch(currentAttack)
        {
            case ZenithAttackType.Melee :
                stateMachine.SwitchState(new ZenithMeleeAttackState(stateMachine));
                return;
            case ZenithAttackType.Shot :
                stateMachine.SwitchState(new ZenithShotAttackState(stateMachine));
                return;
            case ZenithAttackType.Wing :
                stateMachine.SwitchState(new ZenithWingAttackState(stateMachine));
                return;
            case ZenithAttackType.Beam :
                stateMachine.SwitchState(new ZenithBeamAttackState(stateMachine));
                return;
            default :
                stateMachine.SwitchState(new ZenithChasingState(stateMachine));
                return;
        }
    }

    public override void Exit()
    {

    }

    protected bool InWingAttackRange()
    {
        if (stateMachine.Player == null)
        {
            Debug.Log("Cannot Find Player");
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }
        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= WingAttackRange*WingAttackRange;
    }
}