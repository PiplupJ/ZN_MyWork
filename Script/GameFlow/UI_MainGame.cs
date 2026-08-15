/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/12/01
 * 
 * 
 * メインゲームのUI
 * 
 */
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainGame : MonoBehaviour
{
    public static UI_MainGame instance;

    //リスポーンボタン
    [SerializeField] private GameObject respawnButton;
    public event Action Respawn;

    //必殺技ゲージ
    [SerializeField] private GameObject superGauge;
    [SerializeField] private GameObject superGaugeFull;

    //HP
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private GameObject[] HPIcon;

    //ダメージ用HP
    [SerializeField] private GameObject[] damageHPIcon;   //冨岡　拓弥（トミオカ　タクヤ） コード追加　

    int prevHP; //前のHP数値を保存し、今回の体力更新がダメージか増加かを判断

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("UI_MainGame already exists");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        respawnButton.SetActive(false);
        PlayerHP.instance.UpdateHP += UpdateHP;
        foreach (var obj in HPIcon)
        {
            obj.SetActive(false);
        }
        prevHP = 0;
        UpdateHP();
    }

    private void OnDisable()
    {
        PlayerHP.instance.UpdateHP -= UpdateHP;
    }

    //リスポーンボタンクリック
    public void OnClickRespawn()
    {
        Respawn?.Invoke();
        respawnButton.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    //リスポーンボタン有効化
    public void EnableRespawn()
    {
        respawnButton.SetActive(true);
    }

    //必殺技ゲージの更新
    public void UpdateSuperGauge(float _value)
    {
        this.superGauge.GetComponent<Image>().fillAmount = _value;
        if (_value >= 1.0f)
        {
            this.superGaugeFull.SetActive(true);
        }
        else
        {
            this.superGaugeFull.SetActive(false);
        }
    }

    //HP UIの更新
    private void UpdateHP()
    {

        
        int currentHP = PlayerHP.instance.health;

        if(currentHP > prevHP)
        {
            for (int i = 0; i < currentHP; i++)
            {
                HPIcon[i].SetActive(true);
                damageHPIcon[i].SetActive(false);   //冨岡　拓弥（トミオカ　タクヤ） コード追加　
            }
        }
        else if(currentHP < prevHP)
        {
            for (int i = prevHP - 1; i >= currentHP; i--)
            {
                HPIcon[i].SetActive(false);
                damageHPIcon[i].SetActive(true);    //冨岡　拓弥（トミオカ　タクヤ） コード追加　
            }
        }

        prevHP = currentHP;
        
    }


}
