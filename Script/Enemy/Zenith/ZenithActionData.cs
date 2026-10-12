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
    [Header("行動の種類")]
    public ZenithActionType type;

    [Header("行動可能な距離の範囲")]
    public float minRange;
    public float maxRange;

    [Header("基本の出やすさ")]
    public float baseWeight;

    [Header("残りHPによる出やすさの補正")]
    public float lowHealthWeight;

    [Header("攻撃されなかった時間による出やすさの補正")]
    public float passivityWeight;

    [Header("前と同じ行動の出やすさに掛ける倍率")]
    public float repeatPenalty;

    [Header("クールダウン時間")]
    public float cooldown;

}
