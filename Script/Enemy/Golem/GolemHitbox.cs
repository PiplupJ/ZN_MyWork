using UnityEngine;
using System;

public class GolemHitbox : BaseAttackHitBox
{
    public event Action OnAttackHit;

    [SerializeField] private GameObject effectObject;

    public override void Activate()
    {
        base.Activate();
        effectObject.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        effectObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        PlayerHP.instance.TakeDamage(currentAttack);
        OnAttackHit?.Invoke();
        base.Deactivate();
        
    }
}
