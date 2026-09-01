/*
弾発射
作成日：2026/07/13
作成者：ジャンウォンソク
*/
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject firePoint;
   
    public void Fire(ProjectileAttackHitBox prefab, AttackInfo info, Vector3 targetPos)
    {
        Fire(prefab, firePoint.transform.position, info, targetPos);
    }

    public void Fire(ProjectileAttackHitBox prefab, Vector3 pos, AttackInfo info, Vector3 taergetPos)
    {
        ProjectileAttackHitBox projectile = Instantiate(prefab, pos, Quaternion.identity);
        projectile.Init(info, taergetPos);
    }
}
