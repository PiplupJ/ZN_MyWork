using UnityEngine;

public class GolemStateMachine : EnemyStateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [Header("基本能力値")]
    [field: SerializeField] public CharacterController GolemController;
    [field: SerializeField] public float moveSpeed { get; private set; }
    [field: SerializeField] public float rotateSpeed { get; private set; }

    [field: SerializeField] public float ChasingRange { get; private set; }
    [field: SerializeField] public float LaserAttackRange { get; private set; }
    [field: SerializeField] public float MeleeAttackRange { get; private set; }

    [Header("戦闘用")]
    [field: SerializeField] public GolemHP health{ get; private set; }
    [field: SerializeField] public GolemRocketGenerator rocketGenerator{ get; private set; }
    [field: SerializeField] public GameObject target{ get; private set; }
    [field: SerializeField] public GolemHitBoxController hitboxController { get; private set; }
    [field: SerializeField] public GolemCoolDownManager coolManager { get; private set; }
    [field: SerializeField] public GameObject[] RocketFirePoints { get; private set; }
    [field: SerializeField] public GameObject NA_target { get; private set; }

    //シュウが追加しました。
    [SerializeField] private GameObject SuperAttackTarget;

    [SerializeField] public GameObject GolemGore;
    public GameObject Player;

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
        SwitchState(new GolemImpactState(this));
    }

    private void HandleDeath()
    {
        Destroy(this.SuperAttackTarget); //シュウが追加しました
        SwitchState(new GolemDeadState(this));
    }

    public void DestroyGolem()
    {
        SoundPlayer.Instance.PlaySE("G_Destroy");
        SoundPlayer.Instance.StopBGM();
        Instantiate(GolemGore, this.transform.position, Quaternion.identity);
        BattleFinish();
        Destroy(this.gameObject);
    }
}
