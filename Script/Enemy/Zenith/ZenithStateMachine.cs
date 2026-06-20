using UnityEngine;

public class ZenithStateMachine : EnemyStateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [Header("基本能力値")]
    [field: SerializeField] public CharacterController zenithController;
    [field: SerializeField] public ZenithPhase phase { get; private set; }
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public int MeleeAttackPower { get; private set; }
    [field: SerializeField] public int WingAttackPower { get; private set; }
    [field: SerializeField] public int BeamAttackPower { get; private set; }
    [field: SerializeField] public float rotateSpeed { get; private set; }
    [field: SerializeField] public float evadeSpeed { get; private set; }
    [field: SerializeField] public float dashSpeed { get; private set; }

    [field: SerializeField] public float ChasingRange { get; private set; }
    [field: SerializeField] public float MeleeAttackRange { get; private set; }
    [field: SerializeField] public float RangedAttackRange { get; private set; }
    [field: SerializeField] public float EscapeRange { get; private set; }

    [Header("戦闘用")]
    [field: SerializeField] public ZenithHP health{ get; private set; }
    [field: SerializeField] public GameObject target{ get; private set; }
    [field: SerializeField] public ZenithHitBoxManager hitManager{ get; private set; }
	[field: SerializeField] public ZenithCoolDownManager CoolManager{ get; private set; }
    [field: SerializeField] public GameObject[] FirePointsR { get; private set; }
    [field: SerializeField] public GameObject[] FirePointsL { get; private set; }
    [field: SerializeField] public ZenithShotGenerator zsg { get; private set; }
    [field: SerializeField] public EnemySEManager sem { get; private set; }
    
    //シュウが追加しました。
    [SerializeField] private GameObject SuperAttackTarget;

    [SerializeField] public GameObject ZenithGore;
    public GameObject Player;
    //public GameObject Player { get; private set; }

    [HideInInspector]
    public ZenithStates PrevState;

    public GameObject temp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        SwitchState(new ZenithIdleState(this));
    }

    private void OnEnable()
    {
        health.OnTakingDamage += HandleTakingDamage;
        health.OnImpact += HandleImpact;
        health.Death += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnTakingDamage -= HandleTakingDamage;
        health.OnImpact -= HandleImpact;
        health.Death -= HandleDeath;
    }

    //回避できたら回避
    private void HandleTakingDamage()
    {
        if(CoolManager.CanEvade()==true)
        {
            health.AttackEvaded();
            SwitchState(new ZenithEvadingState(this));
        }
    }
 
    private void HandleImpact()
    {   
        hitManager.HitBoxAllDisable();
        Destroy(this.SuperAttackTarget); //シュウが追加しました
        SwitchState(new ZenithImpactState(this));
    }

    private void HandleDeath()
    {
        hitManager.HitBoxAllDisable();
        Destroy(this.SuperAttackTarget); //シュウが追加しました
        SwitchState(new ZenithDeadState(this));
    }

    //Get Attacked
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Weapon")
        {   
            if(Player!=null)
            {
                if(other.TryGetComponent<WeaponController>(out WeaponController weapon))    
                {
                    int damage = weapon.GetPower();
                    health.DealDamage(damage);
                    other.transform.GetComponent<Collider>().enabled = false;
                    weapon.HitHandle();//シュウが追加しました
                }
            }
        }

        //シュウが追加しました。
        else if (other.gameObject.tag == "SuperBullet")
        {
            Destroy(other.gameObject);
            health.DealDamage(5);
        }
    }   
    public void DestroyZenith()
    {
        sem.playEnemySE(EnemySEtype.EnemyDestroy);
        Instantiate(ZenithGore, this.transform.position, Quaternion.identity);
        BattleFinish();
        Destroy(this.gameObject);
    }

    public int GetAttackPower(ZenithAttackType type)
    {
        int power = 0; 
        switch(type)
        {
            case ZenithAttackType.Melee :
                power = this.MeleeAttackPower;
                break;
            case ZenithAttackType.Beam :
                power = this.BeamAttackPower;
                break;
            case ZenithAttackType.Wing :
                power = this.WingAttackPower;
                break;
            default :
                break;
        }
        return power;
    }
}