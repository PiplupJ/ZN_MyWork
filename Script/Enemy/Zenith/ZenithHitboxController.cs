using UnityEngine;
public enum ZenithHitboxType
{
    LeftWing,
    RightWing,
    Laser
}

public class ZenithHitboxController : MonoBehaviour
{
    [SerializeField] private EnemyAttackHitBox WingL1;
    [SerializeField] private EnemyAttackHitBox WingL2;
    [SerializeField] private EnemyAttackHitBox WingR1;
    [SerializeField] private EnemyAttackHitBox WingR2;
    [SerializeField] private EnemyEffectHitbox Laser;

    [SerializeField] private WeaponFX leftWingFX;
    [SerializeField] private WeaponFX rightWingFX;
    public void InitHitboxes(AttackInfo melee, AttackInfo laser)
    {
        WingL1.SetAttack(melee);
        WingL2.SetAttack(melee);
        WingR1.SetAttack(melee);
        WingR2.SetAttack(melee);
        Laser.SetAttack(laser);
    }

    public void ActivateHitbox(ZenithHitboxType type)
    {
        switch(type)
        {
            case ZenithHitboxType.LeftWing :
                WingL1.Activate();
                WingL2.Activate();
                leftWingFX.Begin();
                break;
            case ZenithHitboxType.RightWing :
                WingR1.Activate();
                WingR2.Activate();
                rightWingFX.Begin();
                break;
            case ZenithHitboxType.Laser :
                Laser.Activate();
                break;
            default :
                break;
        }
    }

    public void DeactivateHitbox(ZenithHitboxType type)
    {
        switch(type)
        {
            case ZenithHitboxType.LeftWing :
                WingL1.Deactivate();
                WingL2.Deactivate();
                leftWingFX.End();
                break;
            case ZenithHitboxType.RightWing :
                WingR1.Deactivate();
                WingR2.Deactivate();
                rightWingFX.End();
                break;
            case ZenithHitboxType.Laser :
                Laser.Deactivate();
                break;
            default :
                break;
        }
    }
}
