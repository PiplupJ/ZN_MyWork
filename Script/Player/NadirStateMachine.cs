/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * First update: 2025/12/01
 * Last update : 2026/07/11 by ジャンウォンソク
 * 
 * NADIR（プレイヤー）ステートマシン
 */
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class NadirStateMachine : PlayerStateMachine
{
    public static NadirStateMachine instance { get; private set; }

    [SerializeField] public GameObject player {  get; private set; } //プレイヤー
    [Header("戦闘関連")]
    [SerializeField] public ComboData comboData;
    [SerializeField] public PlayerParry parry;
    [SerializeField] public AttackData counterAttackData;
    public ProjectileAttackHitBox counterBullet;

    //プレイヤーキャラの回転のモード
    public enum RotateMode
    {
        Independent,    //  独立
        WithCamera,     //　カメラの向き
    }

    public RotateMode rotateMode;

    [Header("武器")]
    public WeaponController weapon;
    //ジャンが追加しました。武器の攻撃力をセットするため（25/11/26)

    [Header("Unity セットアップ")]
    public Rigidbody mRigid { get; private set; }
    public CharacterController mCharacterController { get; private set; }

    public Camera mCamera { get; private set; }

    [Header("ジャンプ")]
    //ジャンプ
    public float jumpSpeed = 5.0f;    //ジャンプ速度
    [HideInInspector] public Vector3 previousHorizontalVelocity = Vector3.zero;
    [HideInInspector] public int jumpFrames = 0;
    [HideInInspector] public int jumpFrameAmount = 3;

    [Range(0.0f, 1.0f)]
    public float horizontalJumpFactor = 0.75f;   //ジャンプ時の横移動速度保持比率
    [Header("移動")]
    //移動
    public float walkSpeed = 5.0f;
    public float walkAcceleration = 25f;

    public float runSpeed = 10.0f;
    public float runAcceleration = 40f;

    public float moveDeceleration = 60f;
    [HideInInspector] public float moveSpeed = 0;   //移動速度
    [HideInInspector] public Vector2 moveDirection;      //移動方向
    [HideInInspector] public Vector3 currentVelocity; 

    [Header("回転")]
    //回転
    [SerializeField] public float rotateSpeed = 360.0f;  //回転速度
    [HideInInspector] public Quaternion rotateTarget;

    [Header("ステップ")]
    //ステップ
    public float stepSpeed = 1.0f; //ステップ速度
    public float stepTranslateFrames = 8.0f; //ステップフレーム
    public int stepTotalFrames = 16;
    public int stepBufferFrameAmount = 8;
    

    [HideInInspector] public int stepFrames = 0;
    [HideInInspector] public Vector3 stepDestination;

    [HideInInspector] public int stepBufferFrame = 0;
    [HideInInspector] public bool stepButtonPressedFirst = false;

    public bool canSuperAttack
    {
        get
        {
            return PlayerSuperGauge.instance.canSuperAttack;
        }
    }


    [Header("必殺技弾丸")]
    //弾丸は加速する
    public ProjectileAttackHitBox superBullet;  //必殺技の弾丸
    public AttackData superAttackData;
    public GameObject superTarget;  //必殺技の目標点
    public GameObject firePoint;    //弾丸の発射点
    public ProjectileShooter shooter; //弾の発射
    public float bulletRateMax;     //弾丸の発射頻度の上限
    public float timeToReachBulletRateMax;     //弾丸の発射頻度の上限に到達するまでの時間

    [HideInInspector] public float bulletRate;  //現在の弾丸の発射頻度
    [HideInInspector] public int superAttackFrames; //現在の必殺技フレーム

    [Header("死亡")]
    public float deathAnimationDuration;
    public GameObject playerGore;

    [Header("地面・壁判定")]
    
    [SerializeField] private float slopeTolerance = 0.7f;   //スロープ限界

    [HideInInspector] public Vector3 hitNormal; 

    private List<Collider> contactedGroundList = new List<Collider>();            //接触した地面
    public int contactedGroundListCount { get { return contactedGroundList.Count; } }

    //落下
    public float startFallingTime = 0.0f;
    public float startFallingTimeMax = 0.2f;
    public bool isFalling { get { return startFallingTime >= startFallingTimeMax; } }  //落下中ですが？

    //public bool isGrounded { get { return contactedGroundListCount > 0 && jumpFrames <= 0; } }            //地面にいるか？
    public bool isGrounded { get { return mCharacterController.isGrounded; } }            //地面にいるか？

    //壁判定
    private List<Collider> contactedWallList = new List<Collider>();            //接触している壁
    public bool isWalled { get { return contactedWallList.Count > 0; } }            //壁はあるか？

    public PlayerAnimation mAnimator { get; private set; }      //アニメーター


    //インプット
    public InputAction playerJump;     //ジャンプボタン
    public InputAction playerStep;     //ステップボタン
    public InputAction playerAttack;   //攻撃ボタン
    public InputAction playerSuper;   //必殺技ボタン
    public InputAction playerMove;     //移動スティック
    public InputAction playerParry;   //パリーボタン
    //public InputAction playerRotate;   //回転スティック

    //デバッグ
    [HideInInspector] public string action;

    //小野寺が書き加え------------
    //モーションブラー
    //ステップ時のみ
    private GameObject URP;

    //初期化
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Already have Player Nadir State Machine instance");
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
    }

    private void Start()
    {
        this.mCamera = Camera.main;
        //this.mRigid = GetComponent<Rigidbody>();

        this.mCharacterController = GetComponent<CharacterController>();

        this.mAnimator = GetComponent<PlayerAnimation>();

        if (this.mAnimator == null)
        {
            Debug.Log("Player animator Not Found");
        }

        var playerActionMap = InputManager.Instance.PlayerActionMap;

        this.playerMove = playerActionMap.FindAction("Move");   //移動のボタン（スティック）
        this.playerJump = playerActionMap.FindAction("Jump");   //ジャンプのボタン
        this.playerStep = playerActionMap.FindAction("Step");   //ステップのボタン
        this.playerAttack = playerActionMap.FindAction("Attack");   //攻撃のボタン
        this.playerSuper = playerActionMap.FindAction("Super");   //必殺技のボタン
        this.playerParry = playerActionMap.FindAction("Parry"); //パリーのボタン

        PlayerSuperGauge.instance.ResetSuperGauge();

        PlayerHP.instance.Death += DeathHandle;

        PlayerHP.instance.RegisterParry(parry);
        PlayerHP.instance.ParrySucceeded += ParryHandle;

        weapon.hit += ChargeSuperGauge;

        SwitchState(new NadirIdleState(this));

        //小野寺が書き加え------------
        //モーションブラー
        //ステップ時のみ
        this.URP = GameObject.Find("Global Volume");
    }
    //デリゲート解除
    private void OnDisable()
    {
        PlayerHP.instance.Death -= DeathHandle;
        PlayerHP.instance.ParrySucceeded -= ParryHandle;
        weapon.hit -= ChargeSuperGauge;
    }


    private new void Update()
    {
        base.Update();

        //敵がいないときは無敵状態
        if (GameObject.FindGameObjectWithTag("SuperAttackTarget") == null)
        {
            PlayerHP.instance.SetMuteki();
        }

        //地面から離れた時間を加算
        if (!isGrounded)
        {
            startFallingTime += Time.deltaTime;
        }
        else
        {
            startFallingTime = 0;
        }

        if (playerStep.WasPressedThisFrame())
        {
            URP.GetComponent<URPA_VP>().StepMotionBlur();
        }
    }

    //---------------------------------------------------------

    //接触したのは地面か？
    public bool IsGroundCheck(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        foreach (ContactPoint contact in collision.contacts)
        {
            //ノーマルを取得
            Vector3 normal = contact.normal;

            //ドット
            float dotProduct = Vector3.Dot(normal, Vector3.up);

            //スロープの判定
            if (dotProduct > slopeTolerance)
            {
                return true;
            }
        }
        return false;
    }

    //接触したのは壁か？
    public bool IsWallCheck(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        foreach (ContactPoint contact in collision.contacts)
        {
            //ノーマルを取得
            Vector3 normal = contact.normal;

            //ドット
            float dotProduct = Vector3.Dot(normal, Vector3.up);

            //スロープの判定
            if (dotProduct <= slopeTolerance)
            {
                return true;
            }
        }
        return false;
    }


    //地面にいるか？
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            //地面
            if (IsGroundCheck(collision))
            {
                contactedGroundList.Add(collision.collider);
                //Debug.Log("Enter ground" + collision.gameObject.name);
            }

            //壁
            if (IsWallCheck(collision))
            {
                contactedWallList.Add(collision.collider);
                //Debug.Log("Enter wall" + collision.gameObject.name);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("Platform"))
        {
            //地面
            if (contactedGroundList.Contains(collision.collider))
            {
                contactedGroundList.Remove(collision.collider);
                Debug.Log("Exit Ground" + collision.gameObject.name);

            }

            //壁
            if (contactedWallList.Contains(collision.collider))
            {
                contactedWallList.Remove(collision.collider);
                Debug.Log("Exit Wall" + collision.gameObject.name);
            }

        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        this.hitNormal = hit.normal;
    }

    //必殺技ゲージのリセット
    public void ResetSuperGauge()
    {
        PlayerSuperGauge.instance.ResetSuperGauge();
    }

    //必殺技ゲージのチャージ
    public void ChargeSuperGauge(int _value = 1)
    {
        PlayerSuperGauge.instance.ChargeSuperGauge(_value);
    }

    //必殺技ゲージのアップデート
    public void UpdateSuperGauge()
    {
        PlayerSuperGauge.instance.UpdateSuperGauge();
    }

    //必殺技ゲージのレベルアップ
    public void UpgradeSuperGauge(int _setAmount)
    {
        PlayerSuperGauge.instance.UpgradeSuperGauge(_setAmount);
        UpdateSuperGauge();
    }

    //必殺技ゲージの初期化
    public void InitializeSuperGauge()
    {
        PlayerSuperGauge.instance.InitializeSuperGauge();
    }

    //死亡
    private void DeathHandle()
    {
        SwitchState(new NadirDeadState(this));
    }

    public void DestroyPlayer()
    {
        UI_MainGame.instance.EnableRespawn();
        SoundPlayer.Instance.PlaySE("N_Destroy");
        Instantiate(playerGore, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void ParryHandle(AttackInfo parriedAttack)
    {
        EffectGenerator.Instance.CreateParryEffect(firePoint.transform.position);
        SwitchState(new NadirCounterAttackState(this, parriedAttack));
    }

}
