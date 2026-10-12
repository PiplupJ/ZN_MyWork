//戦闘関係enumやInterfaceなど
//最初作成日：2026/07/12 by ジャンウォンソク
using UnityEngine;

public enum AttackType
{
    Light = 0, 
    Heavy = 1,
    Projectile = 2,
    Counter = 3,
    Super = 4,
    Laser = 5,
    Bomb = 6,
    Slash = 7,
    Supernova = 8
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