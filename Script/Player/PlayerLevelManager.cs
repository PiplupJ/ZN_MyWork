/*
 * 作者：肖 世鴻（シュウ　サイホン）
 * 
 * Last update: 2025/10/10
 * 
 * 
 * プレイヤーのレベルアップを管理するスクリプト
 * 
 */

using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager instance { get; private set; }
    private int XPCount = 0;
    private int playerLevel = 1;

    enum LevelToXP { 
        one = 0,
        two = 10,
        three = 30,
        four = 60,
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("PlayerLevelManager already exists");
            //Destroy(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    //プレイヤーのレベルアップ条件をチェック
    private void levelUpCheck()
    {
        //レベル１
        //レベル2
        if (XPCount == (int)LevelToXP.two)
        {
            playerLevel = 2;
            PlayerHP.instance.UpgradeHP(1);
            Debug.Log("Player Level 2");
        }
        //レベル3
        else if (XPCount == (int)LevelToXP.three) 
        {
            playerLevel = 3;
            PlayerHP.instance.UpgradeHP(1);
            NadirStateMachine.instance.UpgradeSuperGauge(10);
            Debug.Log("Player Level 3");
        }
        //レベル4
        else if (XPCount == (int)LevelToXP.four)
        { 
            playerLevel = 4;
            PlayerHP.instance.UpgradeHP(1);
            NadirStateMachine.instance.UpgradeSuperGauge(5);
            Debug.Log("Player Level 4");
        }
    }

    //プレイヤーXPを増やす
    public void addXP(int _amount = 1)
    {
        XPCount += _amount;
        levelUpCheck();

        Debug.Log("XP count = " + XPCount);
    }

    //プレイヤーレベルを初期化
    public void InitializePlayerLevel()
    {
        playerLevel = 1;
        XPCount = 0;
        PlayerHP.instance.InitializeHP();
        NadirStateMachine.instance.InitializeSuperGauge();
    }
}
