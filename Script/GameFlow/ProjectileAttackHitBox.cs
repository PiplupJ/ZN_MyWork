/*
弾
作成日：2026/07/13
作成者：ジャンウォンソク
*/
using UnityEngine;

public class ProjectileAttackHitBox : BaseAttackHitBox
{
    [SerializeField] private ProjectileMoveData moveData;
    [SerializeField] private EffectId effectId;
    [SerializeField] private float lifetime = 10f;

    float moveSpeed;
    Vector3 moveDirection;

    public void Init(AttackInfo attack, Vector3 moveDirection)
    {
        SetAttack(attack);
        this.moveDirection = moveDirection;
        Activate();
        Destroy(gameObject, moveData.lifetime);

        moveSpeed = moveData.baseSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        if(!TryAttack(other, out var target)){
            return;
        }
        
        EffectGenerator.Instance.CreateEffect(effectId, other.ClosestPoint(transform.position));
        target.TakeDamage(currentAttack);
        Destroy(gameObject);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        lifetime -= deltaTime;

        if(lifetime<=0){
            Destroy(gameObject);
            return;            
        }

        this.transform.position += moveDirection*moveSpeed*deltaTime;

        if(moveSpeed < moveData.maxSpeed){
            moveSpeed += moveData.acceleration*deltaTime;
        }

    }


}
