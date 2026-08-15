/*
 * 作者：張　源碩（ジャン　ウォンソク）
 * 
 * Last update: 2025/11/26
 * 
 * WeaponController.cs
 * 
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : BaseAttackHitBox
{
    public event Action<int> hit;

    private void OnTriggerEnter(Collider other)
    {
        if(!TryAttack(other, out var target)){
            return;
        }
        
        EffectGenerator.Instance.CreateHitEffect(other.ClosestPoint(transform.position));
        target.TakeDamage(currentAttack);
        HitHandle();
        Deactivate();

    }

    //シュウが追加しました
    //ヒット
    public void HitHandle()
    {
        hit?.Invoke(1);
    }
}
