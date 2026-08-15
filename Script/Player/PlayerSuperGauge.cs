using UnityEngine;

public class PlayerSuperGauge : MonoBehaviour
{
    public static PlayerSuperGauge instance { get; private set; }

    public float superGaugeMax;                     //必殺技ゲージの最大値
    [HideInInspector] public float superGaugeValue; //必殺技ゲージの現在値

    private void Awake()
    {
        //シュウが追加しました
        if (instance != null)
        {
            Debug.Log("PlayerSuperGauge already exists");
            //Destroy(this);
            return;
        }
        instance = this;
    }

    //必殺技できる状態？
    public bool canSuperAttack
    {
        get
        {
            return superGaugeValue >= superGaugeMax;
        }
    }

    //必殺技ゲージのリセット
    public void ResetSuperGauge()
    {
        this.superGaugeValue = 0;
        UpdateSuperGauge();
    }

    //必殺技ゲージのチャージ
    public void ChargeSuperGauge(int _value = 1)
    {
        this.superGaugeValue += _value;
        this.superGaugeValue = Mathf.Clamp(this.superGaugeValue, 0, this.superGaugeMax);
        UpdateSuperGauge();
        Debug.Log("Super gauge value = " + this.superGaugeValue);
    }

    //必殺技ゲージのアップデート
    public void UpdateSuperGauge()
    {
        UI_MainGame.instance.UpdateSuperGauge(this.superGaugeValue / this.superGaugeMax);
    }

    //必殺技ゲージのレベルアップ
    public void UpgradeSuperGauge(int _setAmount)
    {
        this.superGaugeMax = _setAmount;
        Debug.Log("superGaugeMax = " + this.superGaugeMax);
        UpdateSuperGauge();
    }

    //必殺技ゲージの初期化
    public void InitializeSuperGauge()
    {
        this.superGaugeMax = 20;
        this.superGaugeValue = 0;
    }

}
