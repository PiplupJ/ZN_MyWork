using UnityEngine;

public class ZenithShotGenerator : MonoBehaviour
{
    public GameObject ZenithShot;
    
    public void ZenithShotAttack(GameObject obj, Transform _target)
    {
       GameObject rocket = Instantiate(ZenithShot, obj.transform.position, obj.transform.rotation);
       var zsc = rocket.GetComponent<ZenithShotController>();
       zsc.Init_Shot(_target);
    }
}
