/*
作成日2026/07/11
作成者：ジャンウォンソク
*/
using UnityEngine;
using System.Collections.Generic;

public class ZenithBrain : MonoBehaviour
{
    [SerializeField] private ZenithActionData[] actions;

    private readonly Dictionary<ZenithActionType, float> actionLog = new Dictionary<ZenithActionType, float>();

    ZenithActionType prevActionType = ZenithActionType.None;

    float noDamageTime = 0;
    float decisionTimer = 0;
    const float DecisionInterval = 2f;
    
    private void Update() {
        if(noDamageTime < float.MaxValue - 1){
            noDamageTime += Time.deltaTime;
        }
        if(decisionTimer > 0){
            decisionTimer -= Time.deltaTime;
        }
    }

    //デリゲートでZenithHealthに連携すること
    public void OnHit()
    {
        noDamageTime = 0;
    }

    public bool CanDecide()
    {
        return decisionTimer<=0;
    }

    public void ScheduleNextDecision()
    {
        decisionTimer = DecisionInterval*Random.Range(0.7f, 1.3f);
    }

    //次の行動を決定
    public ZenithActionType DecideAction(float distanceToPlayer, float healthRatio)
    {
        float totalWeight = 0;

        List<(ZenithActionType type, float weight)> availableActions = new List<(ZenithActionType, float)>();

        //実行可能な行動を調べる
        foreach(var action in actions)
        {
            if(distanceToPlayer < action.minRange || distanceToPlayer > action.maxRange){
                continue;
            }
            if(actionLog.TryGetValue(action.type, out float lastUsedTime)){
                if(Time.time - lastUsedTime < action.cooldown){
                    continue;
                }
            }

            float weight = action.baseWeight 
                            + (1f - healthRatio) * action.lowHealthWeight
                            + noDamageTime * action.passivityWeight;

            if(action.type == prevActionType){
                weight *= action.repeatPenalty;
            }

            if(weight <= 0){
                continue;
            }

            availableActions.Add((action.type, weight));
            totalWeight += weight;
        }

        if(availableActions.Count ==0){
            return ZenithActionType.None;
        }

        //実行可能な行動の中に、各行動の加重値をもとに決める
        float value = Random.value * totalWeight;

        foreach(var(type, weight) in availableActions){
            value -= weight;
            if(value <= 0){
                CommitAction(type);
                return type;
            }
        }
        //不動小数点対策
        var fallback = availableActions[availableActions.Count - 1];
        CommitAction(fallback.type);
        return fallback.type;
    }
    //行動決定を記録する
    private void CommitAction(ZenithActionType type)
    {
        prevActionType = type;
        actionLog[type] = Time.time;
    }
}
