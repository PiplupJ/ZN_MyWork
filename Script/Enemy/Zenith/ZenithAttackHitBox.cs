using UnityEngine;

public class ZenithAttackHitBox : MonoBehaviour
{
    [SerializeField] public ZenithStateMachine stateMachine;
    [SerializeField] public ZenithAttackType type;
    [SerializeField] private AudioClip hitSE;
    private AudioSource aud;

    private void OnEnable() 
    {
        this.aud = GetComponent<AudioSource>();    
    }


    private void OnTriggerEnter(Collider other) 
    {
        //シュウが変更しました
        if (other.CompareTag("Player"))
        {
            this.aud.PlayOneShot(hitSE);
            PlayerHP.instance.DealDamage(stateMachine.GetAttackPower(type));
        }
    }
}
