using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public string motionTag;
    public float activeFrame;
    public float recoverFrame;
    public int damage;
    public bool isParryable;
    public AttackType type;

    public AttackInfo GetAttackInfo(GameObject attacker)
    {
        return new AttackInfo
        {
            attacker = attacker,
            damage = damage,
            isParryable = isParryable,
            type = type
        };
    }
}
