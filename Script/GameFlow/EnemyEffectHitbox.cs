using UnityEngine;
using System;

public class EnemyEffectHitbox : BaseAttackHitBox
{
    public event Action OnAttackHit;

    [SerializeField] private GameObject effectObject;
    [SerializeField] private EffectId effectId;

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
        EffectGenerator.Instance.CreateEffect(effectId, other.ClosestPoint(transform.position));
        OnAttackHit?.Invoke();
        base.Deactivate();
    }
}
