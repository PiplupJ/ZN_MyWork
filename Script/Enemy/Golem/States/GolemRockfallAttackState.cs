using UnityEngine;

public class GolemRockfallAttackState : GolemBaseState
{
    private readonly int AttackHash = Animator.StringToHash("RockfallAttack");

    private const float TransitionDuration = 0.1f;

    private int fireCount;
    private bool [] fired;
    private float startTime;

    public GolemRockfallAttackState(GolemStateMachine stateMachine) : base(stateMachine) { }
   
    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
        fireCount = 0;
        fired = new bool[] { false, false, false, false };
        startTime = 0.54f;
    }

    public override void Tick(float deltaTime)
    {   
        if(fired[fireCount]==false){
            if(GetNormalizedTime(stateMachine.Animator, "RockfallAttack")>=startTime){
                stateMachine.rocketGenerator.GolemRocketShot(stateMachine.RocketFirePoints[fireCount], stateMachine.Player);
                if (stateMachine.sem != null)
                {
                    stateMachine.sem.playEnemySE(EnemySEtype.GolemRocketShot);
                }
                fired[fireCount] = true;
                startTime += 0.1f;
                if(fireCount < fired.Length - 1)
                {
                    fireCount++;
                }
            }
        }

        if(GetNormalizedTime(stateMachine.Animator, "RockfallAttack")>=1){
            stateMachine.SwitchState(new GolemChasingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.coolManager.RockfallAttackCoolDownOn();
    }
}
