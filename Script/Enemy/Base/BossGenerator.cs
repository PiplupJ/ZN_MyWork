using UnityEngine;
using System.Collections;

public class BossGenerator : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private float waitTime = 1.0f;

    private BattleSceneManager bsm;

    void Start()
    {
        StartCoroutine(InstantiateBoss(waitTime));
        this.bsm = GetComponent<BattleSceneManager>();
    }

    private IEnumerator InstantiateBoss(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        GameObject enemy =
        Instantiate(boss, this.transform.position, this.transform.rotation);
        
        EnemyStateMachine esm = enemy.GetComponent<EnemyStateMachine>();

        bsm.InitBattleSceneManager(esm);
    }
}
