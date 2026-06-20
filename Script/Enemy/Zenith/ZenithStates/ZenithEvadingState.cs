using UnityEngine;

public class ZenithEvadingState : ZenithBaseState
{
    private readonly int EvadeHash = Animator.StringToHash("Evading");

    private const float TransitionDuration = 0.1f;

    public ZenithEvadingState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("ZenithEvadingState"+Time.time);
        stateMachine.PrevState = ZenithStates.Evading;
        stateMachine.hitManager.HitBoxAllDisable();
        stateMachine.Animator.CrossFadeInFixedTime(EvadeHash, TransitionDuration);

        stateMachine.sem.playEnemySE(EnemySEtype.ZenithEvade);
    }

    public override void Tick(float deltaTime)
    {
        RotateToPlayer(deltaTime);
        EscapeFromPlayer(deltaTime, stateMachine.evadeSpeed);

        if(GetNormalizedTime(stateMachine.Animator, "Evading")>=1){
            stateMachine.SwitchState(new ZenithChasingState(stateMachine));
            return;
        }

    }

    public override void Exit()
    {
        stateMachine.CoolManager.EvadeCoolDownOn();
    }
    
}
