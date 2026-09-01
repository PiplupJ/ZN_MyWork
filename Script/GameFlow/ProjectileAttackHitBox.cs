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
    [SerializeField] private bool explodeAtTarget;

    float moveSpeed, elapsedTime, lifetime;
    Vector3 currentVelocity;
    Vector3 targetPos;

    bool isInitialized = false;

    public void Init(AttackInfo attack, Vector3 targetPos)
    {
        SetAttack(attack);
        Activate();

        this.targetPos = targetPos;
        this.lifetime = moveData.lifetime;
        moveSpeed = moveData.baseSpeed;
        elapsedTime = 0;
   
        this.currentVelocity = moveData.GetStartVelocity(transform.position, targetPos);
        RotateTowardVelocity();

        isInitialized = true;
    }

    private void OnTriggerEnter(Collider other) {
        
        //衝突したら爆発
        if(TryAttack(other, out var target)){
            target.TakeDamage(currentAttack);
            Explode(other.ClosestPoint(transform.position));
            return;
        }
        Explode(transform.position);
    }

    private void Update()
    {
        if(!isInitialized){ return; }
        float dt = Time.deltaTime;
        elapsedTime += dt;
        lifetime -= dt;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
            return;
        }

        moveSpeed = Mathf.Min(moveSpeed + moveData.acceleration * dt, moveData.maxSpeed);

        currentVelocity = moveData.GetNextVelocity(currentVelocity, transform.position, targetPos, moveSpeed, elapsedTime, dt);
        transform.position += currentVelocity;

        RotateTowardVelocity();

        if(explodeAtTarget && (transform.position - targetPos).sqrMagnitude < 0.1){
            Explode(targetPos);
            SoundPlayer.Instance.PlaySE(SoundType.HitExplosion);
            return;
        }
    }

    private void Explode(Vector3 pos)
    {
        EffectGenerator.Instance.CreateEffect(effectId, pos);
        Destroy(gameObject);
    }

    private void RotateTowardVelocity()
    {
        if (currentVelocity.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(currentVelocity);       
    }

}
