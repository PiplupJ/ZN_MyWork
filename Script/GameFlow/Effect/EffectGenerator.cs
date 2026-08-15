/*
エフェクト生成
作成日：2026/07/13
作成者：ジャンウォンソク
*/
using UnityEngine;
using System.Collections.Generic;

public enum EffectId
{
    //Nadir関連 1x
    NadirCounterHit = 12,
    NadirSuperAttackHit = 13,

    //Golem関連 2x
    GolemMeleeHit = 21,
    GolemLaserHit = 22,

    //Zenith関連 3x
    ZenithMeleeHit = 31,
    ZenithLaserHit = 32,
    ZenithShotHit = 33
}

public class EffectGenerator : MonoBehaviour
{
    public static EffectGenerator Instance;

    [SerializeField] private GameObject parryPrefab;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject shotHitPrefab;

    [SerializeField] private EffectDataBase database;

    private Dictionary <EffectId, EffectData> effectMap;

    void Awake()
    {
        if(Instance!=null && Instance!=this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        effectMap = new Dictionary<EffectId, EffectData>();
        foreach (var effect in database.effects)
        {
            if (!effectMap.TryAdd(effect.id, effect))
                Debug.LogError("IDが重複されているエフェクトがあります"+effect.id,this);
        }
    }

    public void CreateHitEffect(Vector3 pos)
    {
        GameObject go = Instantiate(hitPrefab, pos, Quaternion.identity);
        Destroy(go, 3);
    }

    public void CreateParryEffect(Vector3 pos)
    {
        GameObject go = Instantiate(parryPrefab, pos, Quaternion.identity);
        Destroy(go, 3);
    }

    public void CreateShotHitEffect(Vector3 pos)
    {
        GameObject go = Instantiate(shotHitPrefab, pos, Quaternion.identity);
        Destroy(go, 3);
    }

    public GameObject CreateEffect(EffectId id ,Vector3 pos)
    {
        if(!effectMap.TryGetValue(id, out EffectData data))
        {
            Debug.LogWarning("EffectGenerator:登録されていないEffectID"+id);
            return null;
        }
        return Instantiate(data.prefab, pos, Quaternion.identity);
    }
}
