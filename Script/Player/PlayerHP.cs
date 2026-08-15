/*
 * 作者：張　源碩（ジャン　ウォンソク）
 * 
 * Last update: 2025/11/26
 * 
 * 
 * PlayerHP 
 *
 */
using UnityEngine;
using System;

public class PlayerHP : MonoBehaviour, IDamageable
{
    //シュウが追加しました
    //PlayerHP　の スタティック変数
    public static PlayerHP instance { get; private set; }

    [field: SerializeField] public int maxHealth { get; private set; }

    [HideInInspector] public int health;

    [HideInInspector] public event Action Death; //シュウが追加しました　死亡のイベント

    [HideInInspector] public event Action UpdateHP; //シュウが追加しました　HP更新イベント

    [HideInInspector] public event Action<AttackInfo> ParrySucceeded; //ジャン追加。パリー成功

    [SerializeField] private PlayerParry parry;

    //シュウが追加しました
    private float mutekiTime = 3.0f;
    private float mutekiTimer = 0;
    private bool mutekiState = false;

    private void Awake()
    {
        //シュウが追加しました
        if (instance != null)
        {
            Debug.Log("PlayerHP already exists");
            return;
        }
        instance = this;
    
        health = maxHealth;
    }

    private void Start()
    {
        NewLife();
    }

    private void Update()
    {
        if (mutekiTimer > 0)
        {
            mutekiTimer -= Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        GameManager.instance.Respawn -= PlayerRespawn;
    }

    public void UpgradeHP(int point)
    {
        maxHealth += point;
        health += point;

        //シュウが追加しました
        UpdateHP?.Invoke();
    }

    public void TakeDamage(AttackInfo attack)
    {
        //パリー成功
        if(parry.TryParry(attack)){
            ParrySucceeded?.Invoke(attack);
            return;
        }

        DealDamage(attack.damage);
    }

    public void DealDamage(int damage)
    {
        if (isMuteki())
        {
            return;
        }

        health = Mathf.Max(health - damage, 0);

        //シュウが追加しました
        if (health <= 0)
        {
            if (Death != null)
            {
                Death?.Invoke();
            }
        }

        //シュウが追加しました
        UpdateHP?.Invoke();
        SetMuteki();
    }

    //シュウが追加しました。
    public void Kill()
    {

        health = Mathf.Max(health -　99999, 0);

        //シュウが追加しました
        if (health <= 0)
        {

            Death?.Invoke();
            
        }

        //シュウが追加しました
        UpdateHP?.Invoke();
    }

    //シュウが追加しました
    //プレイヤーの復活処理
    private void PlayerRespawn()
    {
        Death = null;
        health = maxHealth;
    }

    //シュウが追加しました
    //プレイヤーの復活後処理
    public void NewLife()
    {
        GameManager.instance.Respawn += PlayerRespawn;
        UpdateHP?.Invoke();
    }

    //無敵時間を設定
    public void SetMuteki()
    {
        mutekiTimer = mutekiTime;
    }

    public void StartMutekiState()
    {
        mutekiState = true;
    }

    public void EndMutekiState()
    {
        mutekiState = false;
    }

    private bool isMuteki()
    {
        return mutekiTimer > 0 || mutekiState;
    }
    
    public void InitializeHP()
    {
        maxHealth = 1;
        health = maxHealth;
    }

    public void RegisterParry(PlayerParry parry)
    {
        this.parry = parry;
    }
}
