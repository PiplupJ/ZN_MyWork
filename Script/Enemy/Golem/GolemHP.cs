using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class GolemHP : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 200;

    private int health;

    [SerializeField] private int hitSoundCount;

    mainUI mui;

    public event Action OnImpact;
    //public event Action TakingDamage;
    public event Action Death;

    private void Start()
    {
        health = maxHealth;
        GameObject main = GameObject.Find("mainGameUI");
        mui = main.GetComponent<mainUI>();
    }

    public void TakeDamage(AttackInfo attack)
    {
        if (health <= 0) return;   

        if(attack.type == AttackType.Counter){
            OnImpact?.Invoke();
        }
        DealDamage(attack.damage);
    }

    private void DealDamage(int damage)
    {
        if(health<=0){return;}

        health = Mathf.Max(health - damage, 0);
        PlayHitSound();

        if(health<=0)
        {
            if(Death!=null)
            {
                Death.Invoke();
            }
        }
        Debug.Log("GolemHP is"+health);
        mui.BoseHP(maxHealth, health);
  
    }


    private void PlayHitSound()
    {
        string key = "G_Hit"+UnityEngine.Random.Range(1, hitSoundCount).ToString();
        SoundPlayer.Instance.PlaySE(key);
    }
}
