using UnityEngine;
using System;

public class ZenithHP : MonoBehaviour, IDamageable
{
   [SerializeField] private int maxHealth = 200;

    private int health;

    mainUI mui;
    
    public event Action OnTakingDamage;
    public event Action<KnockbackEntry> OnImpact; //変身
    public event Action Death;

    [SerializeField] private int hitSoundCount = 3;

    [SerializeField] private KnockbackTable knockbackTable;

    //ノックバック関係
    private bool hasSuperArmor;

    private void Start()
    {
        health = maxHealth;
        GameObject main = GameObject.Find("mainGameUI");
        mui = main.GetComponent<mainUI>();

        hasSuperArmor = false;
    }

    public void TakeDamage(AttackInfo attack)
    {
        DealDamage(attack.damage);

        if (health > 0)
        {
            if(CheckSuperArmor(attack.type)) return; 
            if(knockbackTable.TryGetEntry(attack.type, out var entry))
            OnImpact?.Invoke(entry);
        }
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

    public void StartSuperArmor()
    {
        hasSuperArmor = true;
    }

    public void EndSuperArmor()
    {
        hasSuperArmor = false;
    }

    //スーパーアーマーを無視する攻撃以外に対してスーパーアーマーチエック
    public bool CheckSuperArmor(AttackType type)
    {
        return  hasSuperArmor
                &&type!=AttackType.Counter
                &&type!=AttackType.Super;
    }
}
