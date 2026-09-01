using UnityEngine;

public class PlayerHitSound
{
    public void PlayHitSound(AttackType attackType)
    {
        if(SoundPlayer.Instance == null) { return; }
        switch(attackType)
        {
            case AttackType.Heavy:
                SoundPlayer.Instance.PlaySE(SoundType.HitHeavy);
                break;
            case AttackType.Slash:
                SoundPlayer.Instance.PlaySE(SoundType.HitSlash);
                break;
            case AttackType.Laser:
                SoundPlayer.Instance.PlaySE(SoundType.HitLaser);
                break;
            case AttackType.Projectile:
                SoundPlayer.Instance.PlaySE(SoundType.HitProjectile);
                break;
            case AttackType.Bomb:
                SoundPlayer.Instance.PlaySE(SoundType.HitExplosion);
                break;
            default :
                break;
        }
    }
}
