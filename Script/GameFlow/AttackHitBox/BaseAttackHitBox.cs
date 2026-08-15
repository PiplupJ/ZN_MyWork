//攻撃用ヒットボックスのスーパークラス
//作成日 2026/07/12 ジャンウォンソク
using UnityEngine;
using System.Collections.Generic;

public class BaseAttackHitBox : MonoBehaviour
{
    protected AttackInfo currentAttack;
    protected HashSet<IDamageable> alreadyHit = new();

    protected Collider col;

    protected void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    public virtual void SetAttack(AttackInfo attack)
    {
        currentAttack = attack;
    }

    public virtual void Activate()
    {
        alreadyHit.Clear();
        col.enabled = true;
    }

    public virtual void Deactivate()
    {
        col.enabled = false;
    }

    protected virtual bool TryAttack(Collider other, out IDamageable target)
    {
        target = null;

        if(currentAttack.attacker == other.gameObject){
            return false;
        }
        target = other.GetComponentInParent<IDamageable>();
        if(target==null){
            return false;
        }
        if(!alreadyHit.Add(target)){
            return false;
        }

        return true;
    }



}
