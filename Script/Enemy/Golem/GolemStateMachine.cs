using UnityEngine;

public class GolemStateMachine : EnemyStateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [Header("基本能力値")]
    [field: SerializeField] public CharacterController GolemController;
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public float rotateSpeed { get; private set; }
    [field: SerializeField] public float moveMultiplier { get; private set; }
    [field: SerializeField] public int AttackPower { get; private set; }

    [field: SerializeField] public float ChasingRange { get; private set; }
    [field: SerializeField] public float LaserAttackRange { get; private set; }
    [field: SerializeField] public float MeleeAttackRange { get; private set; }

    [Header("戦闘用")]
    [field: SerializeField] public GolemHP health{ get; private set; }
    [field: SerializeField] public GolemRocketGenerator rocketGenerator{ get; private set; }
    [field: SerializeField] public GameObject target{ get; private set; }
    [field: SerializeField] public GolemHitBoxManager hitManager { get; private set; }
    [field: SerializeField] public GolemCoolDownManager coolManager { get; private set; }
    [field: SerializeField] public GameObject[] RocketFirePoints { get; private set; }
    [field: SerializeField] public GameObject NA_target { get; private set; }

    [field: SerializeField] public EnemySEManager sem { get; private set; }

    //シュウが追加しました。
    [SerializeField] private GameObject SuperAttackTarget;

    [SerializeField] public GameObject GolemGore;
    public GameObject Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        SwitchState(new GolemIdleState(this));
    }

    private void OnEnable()
    {
        health.OnImpact += HandleImpact;
        health.Death += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnImpact -= HandleImpact;
        health.Death -= HandleDeath;
    }

    private void HandleImpact()
    {   
        hitManager.HitBoxAllDisable();
        SwitchState(new GolemImpactState(this));
    }

    private void HandleDeath()
    {
        hitManager.HitBoxAllDisable();
        Destroy(this.SuperAttackTarget); //シュウが追加しました
        SwitchState(new GolemDeadState(this));
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
    public void DestroyGolem()
    {
        sem.playEnemySE(EnemySEtype.EnemyDestroy);
        Instantiate(GolemGore, this.transform.position, Quaternion.identity);
        BattleFinish();
        Destroy(this.gameObject);
    }

    public int GetAttackPower()
    {
        return this.AttackPower;
    }
}
