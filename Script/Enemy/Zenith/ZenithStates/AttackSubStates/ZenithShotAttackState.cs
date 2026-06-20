using UnityEngine;

public class ZenithShotAttackState : ZenithAttackingState
{
    public ZenithShotAttackState(ZenithStateMachine stateMachine) : base(stateMachine) { }

    private bool fired;

    float timer;

    float shotTime = 0.42f;

    Transform _target;

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ShotAttackHash, TransitionDuration);
        fired = false;
        if (stateMachine.Player == null) { return; }
        _target = stateMachine.Player.transform;

    }

    public override void Tick(float deltaTime)
    {
        timer = GetNormalizedTime(stateMachine.Animator, "Attack_Shot");

        if(timer<shotTime) {RotateToPlayer(deltaTime);}

        if(!fired && timer>=shotTime)
        {
            if (_target == null) return;
            stateMachine.sem.playEnemySE(EnemySEtype.ZenithShot);
            stateMachine.zsg.ZenithShotAttack(stateMachine.temp, _target);

            /*
            for(int i = 0; i < stateMachine.FirePointsL.Length; i++)
            {
                stateMachine.zsg.ZenithShotAttack(stateMachine.FirePointsL[i], _target);
            }
            for(int i = 0; i < stateMachine.FirePointsR.Length; i++)
            {
                stateMachine.zsg.ZenithShotAttack(stateMachine.FirePointsR[i], _target);
            }
            */
            fired = true;
        }
        if(timer >=1){

                stateMachine.CoolManager.ShotAttackCoolDownOn();
                stateMachine.SwitchState(new ZenithChasingState(stateMachine));
                return;
        }
    }

    public override void Exit()
    {

    }
}
