//スーパーノーバー攻撃用
using System.Collections.Generic;
using UnityEngine;

public class SupernovaAttack : MonoBehaviour
{
    [SerializeField] private float maxRadius = 30f; // 最大半径
    [SerializeField] private float expandSpeed = 5f; // 拡大速度
    [SerializeField] private GameObject nova; // スーパーノーバーのビジュアル表現
    [SerializeField] private EffectId effectId; // エフェクトID]
    private float radius = 0f; // 現在の半径
    protected AttackInfo currentAttack; // 攻撃情報

    protected HashSet<IDamageable> alreadyHit = new(); // すでに攻撃した対象を追跡するためのHashSet

    // 攻撃情報を設定する
    public virtual void SetAttack(AttackInfo attack)
    {
        currentAttack = attack;
        nova.SetActive(true);
    }

    //更新、半径拡張
    void FixedUpdate()
    {
        radius = Mathf.Min(radius + expandSpeed * Time.deltaTime, maxRadius);
        expandSpeed+=expandSpeed*Time.deltaTime;
        nova.transform.localScale = new Vector3(radius, radius, radius);

        if(radius >= maxRadius)
        {
            Destroy(gameObject);
        }
    }

    //当たり判定
    private void OnTriggerEnter(Collider other)
    {
        if(TryAttack(other, out IDamageable target))
        {
            EffectGenerator.Instance.CreateEffect(effectId, other.ClosestPoint(transform.position));
            target.TakeDamage(currentAttack);
        }
    }

    //攻撃判定
    protected virtual bool TryAttack(Collider other, out IDamageable target)
    {
        target = null;

        if (currentAttack.attacker == other.gameObject)
        {
            return false;
        }
        target = other.GetComponentInParent<IDamageable>();
        if (target == null)
        {
            return false;
        }
        if (!alreadyHit.Add(target))
        {
            return false;
        }

        return true;
    }
}
