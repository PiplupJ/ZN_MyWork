using UnityEngine;

public class ZenithHitBoxManager : MonoBehaviour
{
    
    public GameObject Wing_L1;
    public GameObject Wing_L2;
    public GameObject Wing_R1;
    public GameObject Wing_R2;
    public GameObject Beam;
    public GameObject[] WingAttack;

    public void HitBoxEnable(ZenithHitBoxes hitbox)
    {
        switch(hitbox)
        {
            case ZenithHitBoxes.WingL1 :
                Wing_L1.GetComponent<Collider>().enabled = true;
                break;
            case ZenithHitBoxes.WingL2 :
                Wing_L2.GetComponent<Collider>().enabled = true;
                break;
            case ZenithHitBoxes.WingR1 :
                Wing_R1.GetComponent<Collider>().enabled = true;
                break;
            case ZenithHitBoxes.WingR2 :
                Wing_R2.GetComponent<Collider>().enabled = true;
                break;
            case ZenithHitBoxes.Beam :
                Beam.SetActive(true);
                break;
            case ZenithHitBoxes.WingAttack :
                for(int i = 0; i < WingAttack.Length; i++)
                {
                    WingAttack[i].GetComponent<Collider>().enabled = true;
                }
                break;
            default :
                break;
        }
    }

    public void HitBoxDisable(ZenithHitBoxes hitbox)
    {
        switch(hitbox)
        {
            case ZenithHitBoxes.WingL1 :
                Wing_L1.GetComponent<Collider>().enabled = false;
                break;
            case ZenithHitBoxes.WingL2 :
                Wing_L2.GetComponent<Collider>().enabled = false;
                break;
            case ZenithHitBoxes.WingR1 :
                Wing_R1.GetComponent<Collider>().enabled = false;
                break;
            case ZenithHitBoxes.WingR2 :
                Wing_R2.GetComponent<Collider>().enabled = false;
                break;
            case ZenithHitBoxes.Beam :
                Beam.SetActive(false);
                break;
            case ZenithHitBoxes.WingAttack :
                for(int i = 0; i < WingAttack.Length; i++)
                {
                    WingAttack[i].GetComponent<Collider>().enabled = false;
                }
                break;
            default :
                break;
        }
    }

    public void HitBoxAllDisable()
    {
        Wing_L1.GetComponent<Collider>().enabled = false;
        Wing_L2.GetComponent<Collider>().enabled = false;
        Wing_R1.GetComponent<Collider>().enabled = false;
        Wing_R2.GetComponent<Collider>().enabled = false;
        Beam.SetActive(false);
        for(int i = 0; i < WingAttack.Length; i++)
        {                
            WingAttack[i].GetComponent<Collider>().enabled = false;
        }
    }
}
