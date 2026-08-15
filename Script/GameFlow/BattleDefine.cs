//戦闘関係enumやInterfaceなど
//最初作成日：2026/07/12 by ジャンウォンソク
using UnityEngine;

public enum AttackType
{
    Light, 
    Heavy,
    Projectile,
    Counter,
    Super,
    Laser
}

public struct AttackInfo
{
    public GameObject attacker;
    public int damage;
    public bool isParryable;
    public AttackType type;
}

public interface IDamageable
{
    void TakeDamage(AttackInfo attack);
}