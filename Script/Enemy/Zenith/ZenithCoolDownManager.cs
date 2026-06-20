using UnityEngine;

public class ZenithCoolDownManager : MonoBehaviour
{
    [Header("クールダウン時間")]

    [field: SerializeField] public float EvadeCoolDownTime { get; private set; }

    [field: SerializeField] public float AttackCoolDownTime { get; private set; }

    [field: SerializeField] public float DashCoolDownTime { get; private set; }

    [field: SerializeField] public float MeleeAttackCoolDownTime { get; private set; }

    [field: SerializeField] public float BeamAttackCoolDownTime { get; private set; }

    [field: SerializeField] public float WingAttackCoolDownTime { get; private set; }

    [field: SerializeField] public float ShotAttackCoolDownTime { get; private set; }
    
    private float EvadeCoolDownTimer = 0.0f;
    private float AttackCoolDownTimer = 0.0f;
    private float DashCoolDownTimer = 0.0f;

    private float MeleeAttackCoolDownTimer = 0.0f;
    private float BeamAttackCoolDownTimer = 0.0f;
    private float WingAttackCoolDownTimer = 0.0f;
    private float ShotAttackCoolDownTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if(CanEvade()==false)
        { EvadeCoolDownTimer -= Time.deltaTime;}

        if(CanAttack()==false)
        { AttackCoolDownTimer -= Time.deltaTime; }

        if(CanDash()==false)
        { DashCoolDownTimer -= Time.deltaTime; }
    }

    public void EvadeCoolDownOn()
    {
        EvadeCoolDownTimer = EvadeCoolDownTime;
    }

    public void AttackCoolDownOn()
    {
        AttackCoolDownTimer = AttackCoolDownTime;
    }

    public void DashCoolDownOn()
    {
        DashCoolDownTimer = DashCoolDownTime;
    }

    public void MeleeAttackCoolDownOn()
    {
        MeleeAttackCoolDownTimer = MeleeAttackCoolDownTime;
    }

    public void BeamAttackCoolDownOn()
    {
        BeamAttackCoolDownTimer = BeamAttackCoolDownTime;
    }

    public void WingAttackCoolDownOn()
    {
        WingAttackCoolDownTimer = WingAttackCoolDownTime;
    }

    public void ShotAttackCoolDownOn()
    {
        ShotAttackCoolDownTimer = ShotAttackCoolDownTime;
    }


    public bool CanEvade()
    {
        return EvadeCoolDownTimer <= 0;
    }

    public bool CanAttack()
    {
        return AttackCoolDownTimer <= 0;
    }

    public bool CanDash()
    {
        return DashCoolDownTimer <= 0;
    }

    public bool CanMeleeAttack()
    {
        return MeleeAttackCoolDownTimer <= 0;
    }

    public bool CanBeamAttack()
    {
        return BeamAttackCoolDownTimer <= 0;
    }

    public bool CanWingAttack()
    {
        return WingAttackCoolDownTimer <= 0;
    }

    public bool CanShotAttack()
    {
        return ShotAttackCoolDownTimer <= 0;
    }
}
