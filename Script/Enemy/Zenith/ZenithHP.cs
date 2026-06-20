using UnityEngine;
using System;
using Random = UnityEngine.Random; 

public class ZenithHP : MonoBehaviour
{
   [SerializeField] private int maxHealth = 200;
   [field: SerializeField] public ZenithPhase phase { get; private set; }

   [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] HitSE;
    int currSE;
    int prevSE;

    private int health;

    mainUI mui;

    private bool evaded;
    
    public event Action OnTakingDamage;
    public event Action OnImpact; //変身
    public event Action Death;

    private void Start()
    {
        health = maxHealth;
        GameObject main = GameObject.Find("mainGameUI");
        mui = main.GetComponent<mainUI>();
    }

    public void DealDamage(int damage)
    {
        if(health<=0){return;}
        
        evaded = false;

        OnTakingDamage.Invoke();

        if(evaded == true)
        {
            Debug.Log("Zenithが攻撃を避けた！");
            return;
        }
        health = Mathf.Max(health - damage, 0);
        PlayHitSound();
        //hitCount++;

        if(health<=maxHealth/10&&this.phase==ZenithPhase.Phase1)
        {
            OnImpact.Invoke();
        }
        else if(health<=0)
        {
            if(Death!=null)
            {
                Death.Invoke();
            }
        }

        Debug.Log(health);

        mui.BoseHP(maxHealth, health);
    }

    public void ResetImpactCount()
    {
        //this.hitCount = 0;
    }

    public void AttackEvaded()
    {
        evaded = true;
    }

    private void PlayHitSound()
    {
        currSE = Random.Range(0, this.HitSE.Length)%this.HitSE.Length;
        if(prevSE == currSE)
        {
            if(prevSE<=0) { currSE +=1;}
            else if(prevSE>=this.HitSE.Length - 1) { currSE -=1;}
        }
        audioSource.PlayOneShot(HitSE[currSE]);

        this.prevSE = this.currSE;
    }
}
