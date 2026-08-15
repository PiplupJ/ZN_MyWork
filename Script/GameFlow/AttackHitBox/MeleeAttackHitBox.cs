using UnityEngine;
using System;

public class MeleeAttackHitBox : BaseAttackHitBox
{
    public event Action OnAttackHit;
    [SerializeField] private EffectId effectId;

    private void OnTriggerEnter(Collider other)
    {
        if(!TryAttack(other, out var target)){
            return;
        }
        
        EffectGenerator.Instance.CreateEffect(effectId, other.ClosestPoint(transform.position));
        target.TakeDamage(currentAttack);
        OnAttackHit?.Invoke();
        Deactivate();
    }
}
