/*
 * 作者:ジャンウォンソク
 * 
 *
 * Last update : 2026/07/11 by ジャンウォンソク
 * 
 * NADIR（プレイヤー）パリーステート
 */
using UnityEngine;

public class NadirParryState : NadirBaseState
{
    private readonly ParryData data;

    public NadirParryState(NadirStateMachine stateMachine) : base(stateMachine) 
    {
        data = stateMachine.parry.parryData;
    }

    private float elapsed;

    enum ParryPhase
    {
        Ready,
        OnParry,
        Recover
    }
    ParryPhase phase;

    public override void Enter()
    {
        elapsed = 0;
        phase = ParryPhase.Ready;
        SoundPlayer.Instance.PlaySE("N_Parry");
        stateMachine.mAnimator.Parry();
    }

    public override void Tick(float deltaTime)
    {
        elapsed += deltaTime;

        switch(phase)
        {
            //パリー準備
            case ParryPhase.Ready :
                if(elapsed>= data.windowStart){
                    stateMachine.parry.OpenWindow();
                    phase = ParryPhase.OnParry;
                }
                break;
            //パリー中
            case ParryPhase.OnParry :
                if(elapsed>= data.windowEnd){
                    stateMachine.parry.CloseWindow();
                    phase = ParryPhase.Recover;
                }
                break;
            //パリー終了以降待機時間
            case ParryPhase.Recover :
                if(elapsed >= data.stateEnd){
                    stateMachine.SwitchState(new NadirIdleState(stateMachine));
                    return;
                }
                break;
            default :
                break;
        }
    }

    public override void FixedTick(float deltaTime)
    {

    }

    public override void Exit()
    {
        stateMachine.parry.CloseWindow();
    }
}
