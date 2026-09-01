/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2026/08/22 by ジャンウォンソク
 * 
 * 
 * カメラの制御
 * 
 */

using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    //カメラのモード
    public enum CameraMode
    {
        Normal,
        SuperAttack,
        LockOn,
        Death,
    }

    public CameraMode mode;

    private InputAction viewRotate;
    private InputAction viewLockOn;

    //カメラ/視点
    Camera mCamera;

    private Vector3 dollyDirection;
    private float currentCameraDistance;

    [SerializeField] private float cameraRightShift_Normal = 0.8f;
    [SerializeField] private float cameraUpShift_Normal = 0f;
    [SerializeField] private float cameraForwardShift_Normal = 0f;

    [SerializeField] private float cameraRightShift_SuperAttack = 0.8f;
    [SerializeField] private float cameraUpShift_SuperAttack = 0f;
    [SerializeField] private float cameraForwardShift_SuperAttack = 0f;

    [SerializeField] private float playerRightShift_Normal = 0.8f;
    [SerializeField] private float playerTargetUpShift_Normal = 0f;

    [SerializeField] private float rotateSpeed = 5.0f;
    [SerializeField] private GameObject playerTarget;           //カメラ目標
    //[SerializeField] private Transform lockOnTarget;           //ロックオン目標
    public GameObject currentTarget { get; private set; }                           //カメラ現在の目標

    private Vector2 cameraRotateDir;    //カメラ回転方向（左・右）

    //ノーマルカメラ
    [SerializeField] private float normalZoom = 3;
    //必殺技カメラ
    [SerializeField] private float superAttackZoom = 2;
    //死亡カメラ
    [SerializeField] private float deathZoom = 3;
    
    [SerializeField] private float minXCameraRotation = -35.0f;
    [SerializeField] private float maxXCameraRotation = 65.0f;
    [SerializeField] private float minDriftLimitX = 0.25f;
    [SerializeField] private float minDriftLimitY = 0.25f;

    //ロックオンカメラ用
    private float LockOnAngleX;
    private float LockOnAngleY;

    //Cinemachine
    public CinemachineCamera CM;
    //private CinemachinePanTilt CMPanTilt;

    private OccluderFader occluderFader;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("More than one 'CameraController' in scene");
            return;
        }

        Application.targetFrameRate = 60;
        Instance = this;
    }

    private void Start()
    {
        if (InputManager.Instance == null)
        {
            Debug.Log("No InputManager");
            return;
        }

        var playerActionMap = InputManager.Instance.PlayerActionMap;
        this.viewRotate = playerActionMap.FindAction("Look");   //カメラ回転のボタン（スティック）
        this.viewLockOn = playerActionMap.FindAction("LockOn");

        mCamera = Camera.main;
        mCamera.transform.LookAt(playerTarget.transform);   //カメラを目標に向ける

        //カメラをシフトする
        mCamera.transform.SetLocalPositionAndRotation(new Vector3(cameraRightShift_Normal, cameraUpShift_Normal, cameraForwardShift_Normal), Quaternion.identity);
        playerTarget.transform.SetLocalPositionAndRotation(new Vector3(playerRightShift_Normal, playerTargetUpShift_Normal, 0), Quaternion.identity);

        //カメラの初期距離と方向
        dollyDirection = mCamera.transform.localPosition.normalized;
        currentCameraDistance = mCamera.transform.localPosition.magnitude;

        mode = CameraMode.Normal;

        currentTarget = playerTarget;

        LockOnAngleX = 0;
        LockOnAngleY = 0;

        this.transform.position = playerTarget.transform.position;

        PlayerHP.instance.Death += PlayerDeathHandle;

        occluderFader = new OccluderFader();

        occluderFader.Initialize(playerTarget.transform);
    }

    private void OnDisable()
    {
        PlayerHP.instance.Death -= PlayerDeathHandle;
        occluderFader.Clear();
    }

    private void Update()
    {

        if (viewLockOn.WasPressedThisFrame())
        {
            switch (mode)
            {
                case CameraMode.Normal:
                    SetLockOn();
                    break;

                case CameraMode.SuperAttack:
                    //SetNormal();
                    break;

                case CameraMode.LockOn:
                    mode = CameraMode.Normal;
                    SetNormal();
                    break;
                default:
                    break;
            }
        }
    }

    private void FixedUpdate()
    {

        switch (mode) {
            case CameraMode.Normal:
                if (GameObject.FindGameObjectWithTag("Player") != null)
                {
                    CameraTranslate(NadirStateMachine.instance.transform.position);
                }
                CameraRotate();
                SetCameraZoom(normalZoom);
                CameraLookAt();

                break;

            case CameraMode.SuperAttack:
                
                if (currentTarget == null)
                {
                    currentTarget = playerTarget;
                    mode = CameraMode.Normal;
                    break;
                }

                if (GameObject.FindGameObjectWithTag("Player") != null)
                {
                    CameraTranslate(NadirStateMachine.instance.transform.position);
                }
                CameraRotate();
                SetCameraZoom(superAttackZoom);
                CameraLookAt();
                break;
            case CameraMode.LockOn:

                if (currentTarget == null)
                {
                    currentTarget = playerTarget;
                    mode = CameraMode.Normal;
                    break;
                }

                if (GameObject.FindGameObjectWithTag("Player") != null)
                {
                    CameraTranslate(NadirStateMachine.instance.transform.position);
                }
                CameraRotate();
                SetCameraZoom(normalZoom);
                CameraLookAt();

                break;

            case CameraMode.Death:
                SetCameraZoom(deathZoom);
                break;
            default :
                break;
        }

        //カメラの向きを目標に向ける
    }

    private void LateUpdate() {

        if(mode == CameraMode.Death)
        {
            return;
        }     

        occluderFader.Tick(mCamera.transform.position);
    }

    //カメラ回転処理
    private void CameraRotate()
    {
        switch (mode)
        {
            case CameraMode.Normal:
                CameraRotate_Independent();
                break;
            
            case CameraMode.LockOn:
                CameraRotate_LockOn();
                break;
            
            case CameraMode.SuperAttack:
                CameraRotate_LockOn();
                break;
            default :
                break;
        }
    }

    //プレイヤーの向きから独立する
    private void CameraRotate_Independent()
    {
        if (!viewRotate.IsPressed()) { return; }

        cameraRotateDir = viewRotate.ReadValue<Vector2>();

        //Y軸回転（横）
        if (cameraRotateDir.x > minDriftLimitX || cameraRotateDir.x < -minDriftLimitX)
        {  //小さい過ぎるドリフトを無視
            this.transform.RotateAround(this.transform.position, Vector3.up,
                    this.rotateSpeed * cameraRotateDir.x);
        }

        //X軸回転（上下）
        Vector3 currentRotation = this.transform.localEulerAngles;

        //Debug.Log(cameraRotateDir.y);

        if (cameraRotateDir.y > minDriftLimitY || cameraRotateDir.y < -minDriftLimitY) 
        {  //小さい過ぎるドリフトを無視
            if (currentRotation.x > 180) currentRotation.x -= 360;

            if ((currentRotation.x - this.rotateSpeed * cameraRotateDir.y) >= minXCameraRotation
                && (currentRotation.x - this.rotateSpeed * cameraRotateDir.y) <= maxXCameraRotation)    //上下回転の制限
            {

                this.transform.RotateAround(playerTarget.transform.position, this.transform.right,
                    -this.rotateSpeed * cameraRotateDir.y);

            }
        }

    }

    //ロックオン
    private void CameraRotate_LockOn()
    {
        //Y軸回転（横）
        Vector3 player2TargetDir = (currentTarget.transform.position
            - playerTarget.transform.position).normalized;                     //プレイヤーからロックオンした敵への方向

        Vector3 player2TargetYAxis = new Vector3(player2TargetDir.x, 0, player2TargetDir.z);
        Vector3 currentYAxisRotate = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);

        LockOnAngleY = Vector3.SignedAngle(currentYAxisRotate, player2TargetYAxis, this.transform.up); //横回転のアングル

        float rotateAngleY;

        if (LockOnAngleY < 0.1f && LockOnAngleY > -0.1f)
        {
            rotateAngleY = 0;
        }
        else
        {
            rotateAngleY = LockOnAngleY * 0.9f;
        }

            this.transform.RotateAround(this.transform.position, Vector3.up, rotateAngleY);

        //X軸回転（上下）

        Vector3 camera2PlayerDir = (playerTarget.transform.position 
            - mCamera.transform.position).normalized;

        Vector3 player2TargetXAxis = new Vector3(0, 
                                    (float)System.Math.Round((double)player2TargetDir.y, 2), 
                                    (float)System.Math.Round((double)player2TargetDir.z, 2));

        Vector3 currentXAxisRotate = new Vector3(0,
                                    (float)System.Math.Round((double)mCamera.transform.forward.y, 2),
                                    (float)System.Math.Round((double)mCamera.transform.forward.z, 2));

        LockOnAngleX = (float)System.Math.Round((double)Vector3.SignedAngle(currentXAxisRotate, player2TargetXAxis, playerTarget.transform.right), 2); //上下回転のアングル

        if (LockOnAngleX >= 360f)
        {
            LockOnAngleX -= 360f;
        }
        else if (LockOnAngleX <= -360f)
        {
            LockOnAngleX += 360f;
        }

        float rotateAngleX;

        if (LockOnAngleX < 0.1f && LockOnAngleX > -0.1f)
        {
            rotateAngleX = 0;
        }
        else
        {
            rotateAngleX = LockOnAngleX * Time.deltaTime;
        }


        {
            this.transform.RotateAround(playerTarget.transform.position, this.transform.right, rotateAngleX);
        }

    }

    //カメラ座標の設定
    private Vector3 CameraPositon
    {
        set
        {
            this.transform.position = value;
        }
    }

    //カメラ座標の設定
    private void CameraTranslate(Vector3 _position)
    {
        this.transform.position = _position;

    }

    //カメラの見る方向
    private void CameraLookAt()
    {
        mCamera.transform.LookAt(currentTarget.transform);
    }

    //カメラのズーム
    private void SetCameraZoom(float _value)
    {
        mCamera.fieldOfView = _value;

        CM.GetComponent<CinemachineThirdPersonFollow>().CameraDistance = _value;
    }

    //ターゲットにロックオン
    private void SetLockOnTarget()
    {
        GameObject lockOnObject = null;

        {
            float minDistance = Mathf.Infinity; //最短直線距離の敵をロックオン
            foreach ( GameObject gameObject in GameObject.FindGameObjectsWithTag("EnemyCamera"))
            {
                float distance = Vector3.Distance(this.transform.position, gameObject.transform.position);
                if (distance  < minDistance)
                {
                    minDistance = distance;
                    currentTarget = gameObject;
                }
            }
        }

        if (lockOnObject != null)
        {
            currentTarget = lockOnObject;
        }
    }

    //プレイヤーにロックオン
    private void SetLockOnPlayer()
    {
        currentTarget = playerTarget;
    }

    //モードセット

    //モード：通常
    public void SetNormal()
    {
        mode = CameraMode.Normal;
        SetLockOnPlayer();
    }

    //モード：ロックオン
    public void SetLockOn()
    {
        mode = CameraMode.LockOn;
        SetLockOnTarget();
    }

    //モード：必殺技
    public void SetSuperAttack()
    {
        mode = CameraMode.SuperAttack;
        SetLockOnTarget();
    }

    //プレイヤー死亡
    private void PlayerDeathHandle()
    {
        mode = CameraMode.Death;
    }
}
