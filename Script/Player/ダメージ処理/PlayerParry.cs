//プレイヤーのパリー管理
//作成日 2026/07/12 作成者　ジャンウォンソク
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [field: SerializeField] public ParryData parryData { get; private set; }

    private bool isWindowOpen;

    //パリーを試す
    public bool TryParry(AttackInfo attack)
    {
        if(!isWindowOpen){
            return false;
        }
        if(!attack.isParryable){
            return false;
        }

        return true;
    }

    //パリー開始
    public void OpenWindow(){
        isWindowOpen = true;
    }   
    //パリー終了
    public void CloseWindow(){
        isWindowOpen = false;
    }
}
