using UnityEngine;
using System;

public class EnemyAttackHitBox : BaseAttackHitBox
{
    public event Action OnAttackHit;
    [SerializeField] private EffectId effectId;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        PlayerHP.instance.TakeDamage(currentAttack);
        EffectGenerator.Instance.CreateEffect(effectId, other.ClosestPoint(transform.position));
        OnAttackHit?.Invoke();
        base.Deactivate();
    }
}
