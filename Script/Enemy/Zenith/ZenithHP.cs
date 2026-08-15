using UnityEngine;
using System;

public class ZenithHP : MonoBehaviour, IDamageable
{
   [SerializeField] private int maxHealth = 200;

    private int health;

    mainUI mui;
    
    public event Action OnTakingDamage;
    public event Action OnImpact; //変身
    public event Action Death;

    [SerializeField] private int hitSoundCount = 3;

    private void Start()
    {
        health = maxHealth;
        GameObject main = GameObject.Find("mainGameUI");
        mui = main.GetComponent<mainUI>();
    }

    public void TakeDamage(AttackInfo attack)
    {
        if(attack.type == AttackType.Counter){
            OnImpact?.Invoke();
        }
        DealDamage(attack.damage);
    }

    private void DealDamage(int damage)
    {
        if(health<=0){return;}
        
        health = Mathf.Max(health - damage, 0);

        OnTakingDamage?.Invoke();
        PlayHitSound();

        if(health<=0)
        {
            Death?.Invoke();
        }

        mui.BoseHP(maxHealth, health);
    }

    private void PlayHitSound()
    {
        string se = "Z_Hit"+UnityEngine.Random.Range(1, hitSoundCount).ToString();
        SoundPlayer.Instance.PlaySE(se);       
    }

    public float GetHealthRatio()
    {
        return (float)health/(float)maxHealth;
    }
}
