/*
弾発射
作成日：2026/07/13
作成者：ジャンウォンソク
*/
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    public GameObject firePoint;
   
    public void Fire(ProjectileAttackHitBox prefab, AttackInfo info, Vector3 dir)
    {
            ProjectileAttackHitBox projectile = Instantiate(prefab, firePoint.transform.position, Quaternion.LookRotation(dir));
            projectile.Init(info, dir);

    }

    public void Fire(ProjectileAttackHitBox prefab, Vector3 pos, AttackInfo info, Vector3 dir)
    {
        ProjectileAttackHitBox projectile = Instantiate(prefab, pos, Quaternion.LookRotation(dir));
        projectile.Init(info, dir);
    }
}
