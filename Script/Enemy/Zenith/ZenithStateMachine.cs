using UnityEngine;

public class ZenithStateMachine : EnemyStateMachine
{
    [Header("基本能力値")]
    [field: SerializeField] public CharacterController zenithController;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public Vector3 currentVelocity;
    [field: SerializeField] public float moveAcceleration { get; private set; }
    [field: SerializeField] public float moveDeceleration { get; private set; }

    [field: SerializeField] public float chaseSpeed { get; private set; }
    [field: SerializeField] public float rotateSpeed { get; private set; }
    [field: SerializeField] public float backStepSpeed { get; private set; }
    [field: SerializeField] public float dashSpeed { get; private set; }

    [field: SerializeField] public float ChasingRange { get; private set; }
    [field: SerializeField] public float targetRange { get; private set; }
    
    [field: SerializeField] public float sightAngle { get; private set; }

    [Header("戦闘用")]
    [field: SerializeField] public ZenithHP health{ get; private set; }
    [field: SerializeField] public GameObject target{ get; private set; }
    [field: SerializeField] public ZenithHitboxController hitboxController { get; private set; }
    [field: SerializeField] public ZenithBrain brain{ get; private set; }

    [SerializeField] private AttackData meleeAttackData;
    [SerializeField] private AttackData laserAttackData;  
    [SerializeField] public AttackData shotAttackData;

    [field: SerializeField] public GameObject[] FirePoints { get; private set; }

    [field: SerializeField] public ProjectileShooter shooter { get; private set; }
    [field: SerializeField] public ProjectileAttackHitBox shotBullet { get; private set; }

    [field: SerializeField] public ZenithAnimation mAnimator { get; private set; }
    
    //シュウが追加しました。
    [SerializeField] private GameObject SuperAttackTarget;

    [SerializeField] public GameObject ZenithGore;
    public GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        hitboxController.InitHitboxes(meleeAttackData.GetAttackInfo(this.gameObject), laserAttackData.GetAttackInfo(this.gameObject));

        SwitchState(new ZenithIdleState(this));
    }

    private void OnEnable()
    {
        health.OnImpact += HandleImpact;
        health.Death += HandleDeath;
        health.OnTakingDamage += brain.OnHit;
    }

    private void OnDisable()
    {
        health.OnImpact -= HandleImpact;
        health.Death -= HandleDeath;
        health.OnTakingDamage -= brain.OnHit;
    }

 
    private void HandleImpact()
    {   
        SwitchState(new ZenithImpactState(this));
    }

    private void HandleDeath()
    {
        Destroy(this.SuperAttackTarget); //シュウが追加しました
        SwitchState(new ZenithDeadState(this));
    }

    public void DestroyZenith()
    {
        //sem.playEnemySE(EnemySEtype.EnemyDestroy);
        Instantiate(ZenithGore, this.transform.position, Quaternion.identity);
        BattleFinish();
        Destroy(this.gameObject);
    }
}