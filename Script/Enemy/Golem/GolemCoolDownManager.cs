using UnityEngine;

public class GolemCoolDownManager : MonoBehaviour
{
    [field: SerializeField] public float MeleeAttackCoolDownTime { get; private set; }
    [field: SerializeField] public float RockfallAttackCoolDownTime { get; private set; }
    [field: SerializeField] public float LaserCoolDownTime { get; private set; }
    

    private float MeleeAttackCoolDownTimer = 0.0f;
    private float RockfallAttackCoolDownTimer = 0.0f;
    private float LaserCoolDownTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(!CanMeleeAttack())
        {
            MeleeAttackCoolDownTimer -= Time.deltaTime;
        }
        if(!CanRockfallAttack())
        {
            RockfallAttackCoolDownTimer -= Time.deltaTime;
        }
        if(!CanLaserAttack())
        {
            LaserCoolDownTimer -= Time.deltaTime;
        }
    }

    public bool CanMeleeAttack()
    {
        return MeleeAttackCoolDownTimer <=0;
    }

    public void MeleeAttackCoolDownOn()
    {
        MeleeAttackCoolDownTimer = MeleeAttackCoolDownTime;
    }

    public bool CanRockfallAttack()
    {
        return RockfallAttackCoolDownTimer <=0;
    }

    public void RockfallAttackCoolDownOn()
    {
        RockfallAttackCoolDownTimer = RockfallAttackCoolDownTime;
    }

    public bool CanLaserAttack()
    {
        return LaserCoolDownTimer <=0;
    }

    public void LaserAttackCoolDownOn()
    {
        LaserCoolDownTimer = LaserCoolDownTime;
    }
}
