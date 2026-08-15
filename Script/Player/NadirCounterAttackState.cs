//パリーカウンタ攻撃
using UnityEngine;

public class NadirCounterAttackState : NadirBaseState
{
    public NadirCounterAttackState(NadirStateMachine stateMachine, AttackInfo parriedAttack) : base(stateMachine) 
    {
        this.parriedAttack = parriedAttack;
    }

    private AttackInfo parriedAttack;

    enum CounterPhase
    {
        Ready,
        Active,
        Recover
    }
    CounterPhase phase;

    //無敵に設定、攻撃者へ向く
    public override void Enter()
    {
        PlayerHP.instance.StartMutekiState();
        stateMachine.mAnimator.Counter();
        phase = CounterPhase.Ready;
        PlayerRotateTowards(parriedAttack.attacker.transform);
    }

    public override void Tick(float deltaTime)
    {
        float t = stateMachine.mAnimator.GetNormalizedTime("Counter");

        switch(phase)
        {
            case CounterPhase.Ready :
                if(t>=stateMachine.counterAttackData.activeFrame){
                    if(TryGetDirectionToTarget(parriedAttack.attacker, out Vector3 dir)){
                        SoundPlayer.Instance.PlaySE("N_Counter");
                        //カウンタ攻撃を発射
                        stateMachine.shooter.Fire(stateMachine.counterBullet, stateMachine.counterAttackData.GetAttackInfo(stateMachine.gameObject) , dir);
                    }
                    phase = CounterPhase.Active;
                }
                break;
            case CounterPhase.Active :
                if(t>=stateMachine.counterAttackData.recoverFrame){
                    phase = CounterPhase.Recover;
                }
                break;
            case CounterPhase.Recover :
                if(t>=1.0f){
                    //ステート終了。待機状態へ
                    stateMachine.SwitchState(new NadirIdleState(stateMachine));
                }
                break;
            default:
                break;
        }
    }

    public override void FixedTick(float deltaTime)
    {

    }

    public override void Exit()
    {
        PlayerHP.instance.EndMutekiState();
    }
}