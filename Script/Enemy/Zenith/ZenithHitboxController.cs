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

    private void OnEnable()
    {
        WingL1.OnAttackHit += PlayMeleeSFX;
        WingL2.OnAttackHit += PlayMeleeSFX;
        WingR1.OnAttackHit += PlayMeleeSFX;
        WingR2.OnAttackHit += PlayMeleeSFX;
        Laser.OnAttackHit += PlayLaserHitSFX;
    }

    private void OnDisable() 
    {
        WingL1.OnAttackHit -= PlayMeleeSFX;
        WingL2.OnAttackHit -= PlayMeleeSFX;
        WingR1.OnAttackHit -= PlayMeleeSFX;
        WingR2.OnAttackHit -= PlayMeleeSFX;
        Laser.OnAttackHit -= PlayLaserHitSFX;
    }

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
                break;
            case ZenithHitboxType.RightWing :
                WingR1.Activate();
                WingR2.Activate();
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
                break;
            case ZenithHitboxType.RightWing :
                WingR1.Deactivate();
                WingR2.Deactivate();
                break;
            case ZenithHitboxType.Laser :
                Laser.Deactivate();
                break;
            default :
                break;
        }
    }
    
    private void PlayMeleeSFX()
    {
        string key = "Z_AttackHit"+(UnityEngine.Random.Range(0, 2)+1).ToString();
        SoundPlayer.Instance.PlaySE(key);       
    }

    private void PlayLaserHitSFX()
    {
        SoundPlayer.Instance.PlaySE("LaserHit");
    }
}
