using UnityEngine;

//行動の種類
public enum ZenithActionType
{
    None,
    Dash,
    BackStep,
    Melee,
    DoubleMelee,
    Laser,
    Wing,
    Shot
}
//行動データ
[CreateAssetMenu(fileName = "ZenithActionData", menuName = "Scriptable Objects/ZenithActionData")]
public class ZenithActionData : ScriptableObject
{
    [Header("行動")]
    public ZenithActionType type;

    [Header("行動の距離条件")]
    public float minRange;
    public float maxRange;

    [Header("基本加重値")]
    public float baseWeight;

    [Header("体力状態による加重値")]
    public float lowHealthWeight;

    [Header("プレイヤーが逃げ続ける場合の加重値")]
    public float passivityWeight;

    [Header("直前に同じ行動をした場合の加重値")]
    public float repeatPenalty;

    [Header("クールダウン時間")]
    public float cooldown;

}
