using UnityEngine;

public class GolemHitBoxController : MonoBehaviour
{
    public MeleeAttackHitBox MeleeHitBox;
    public EnemyEffectHitbox LaserHitBox;

    [SerializeField] private AttackData MeleeAttackData;
    [SerializeField] private AttackData LaserAttackData;

    private void Start()
    {
        MeleeHitBox.SetAttack(MeleeAttackData.GetAttackInfo(this.gameObject));
        LaserHitBox.SetAttack(LaserAttackData.GetAttackInfo(this.gameObject));
    }

    private void OnEnable()
    {
        LaserHitBox.OnAttackHit += PlayLaserHitSFX;
    }

    private void OnDisable()
    {
        LaserHitBox.OnAttackHit -= PlayLaserHitSFX;
    }

    public void HitBoxEnable(GolemAttackType hitbox)
    {
        switch(hitbox)
        {
            case GolemAttackType.Melee :
                MeleeHitBox.Activate();
                break;
            case GolemAttackType.Laser :
                LaserHitBox.Activate();
                break;
            default :
                break;
        }
    }
    public void HitBoxDisable(GolemAttackType hitbox)
    {
        switch(hitbox)
        {
            case GolemAttackType.Melee :
                MeleeHitBox.Deactivate();
                break;
            case GolemAttackType.Laser :
                LaserHitBox.Deactivate();
                break;
            default :
                break;
        }
    }

    public void HitBoxAllDisable()
    {
        MeleeHitBox.Deactivate();
        LaserHitBox.Deactivate();
    }

    private void PlayLaserHitSFX()
    {
        SoundPlayer.Instance.PlaySE("LaserHit");
    }
}

public enum GolemAttackType
{
    Melee,
    Laser
}
