using UnityEngine;

public class NadirUltimateKit : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] effects;
    public AttackData ultimateAttackdata;
    public ProjectileAttackHitBox ultimateProjectile;

    public void PlayEffects()
    {
        foreach(var p in effects)
        {
            p.Play();
        }
    }

    public void StopEffects()
    {
        foreach(var p in effects)
        {
            p.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
