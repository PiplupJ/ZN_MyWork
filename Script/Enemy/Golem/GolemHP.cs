using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class GolemHP : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;

    private int health;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] HitSE;
    int currSE;
    int prevSE;

    mainUI mui;

    public int ImpactCount = 3;

    private int hitCount = 0;

    public event Action OnImpact;
    //public event Action TakingDamage;
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

        //if(lastHitFrame == Time.frameCount){return;}

        //lastHitFrame = Time.frameCount;
        health = Mathf.Max(health - damage, 0);
        PlayHitSound();
        
        hitCount++;

        if(health<=0)
        {
            if(Death!=null)
            {
                Death.Invoke();
            }
        }
        else if(hitCount==ImpactCount)
        {
            OnImpact.Invoke();
        }

        Debug.Log(health);

        mui.BoseHP(maxHealth, health);
  
    }

    public void ResetImpactCount()
    {
        this.hitCount = 0;
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
