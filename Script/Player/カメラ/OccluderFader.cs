//カメラとプレイヤー間の障害物を透明化
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
public class OccluderFader
{
    private class Entry
    {
        public bool wasHitThisFrame;
        public ShadowCastingMode originalShadow;
    }

    private readonly Dictionary<Renderer, Entry> entries = new Dictionary<Renderer, Entry>();

    private readonly List<Renderer> removeBuffer = new List<Renderer>();
    private Transform target;

    private float hitRadius = 0.3f;
    private RaycastHit[] hitResults = new RaycastHit[15];

    private float targetPadding = 0.5f;

    //CameraControllerのStart()でPlayerをTargetにした以降数値を渡す
    public void Initialize(Transform target)
    {
        this.target = target;

        //Debug.Log("OcclusionFader generated");
    }

    //CameraControllerのlateUpdateで呼び出す
    public void Tick(Vector3 cameraPos)
    {
        if(target==null) { return;}
        
        DetectOccluder(cameraPos);

        UpdateFade();
    }

    private void DetectOccluder(Vector3 cameraPos)
    {
        foreach(var e in entries.Values)
        {
            e.wasHitThisFrame = false;
        }

        Vector3 targetPos = GetTargetPoint();
        
        Vector3 dir = (targetPos - cameraPos).normalized;

        float maxDist = (targetPos - cameraPos).magnitude - targetPadding;

        if(maxDist <=0) return;
        
        //床の裏面を認識するため
        bool prevBackfaces = Physics.queriesHitBackfaces;   //現在のBackfaces設定を読む
        Physics.queriesHitBackfaces = true; //一時的に活性化
        int hitCount = Physics.SphereCastNonAlloc(cameraPos, hitRadius, dir, hitResults, maxDist); //カメラと対象間のオブジェクト数
        Physics.queriesHitBackfaces = prevBackfaces;    //Backfaces設定を元に戻す

        //カメラとプレイヤー間の障害物を確認
        for(int i = 0; i < hitCount; i++)
        {
            RaycastHit hit = hitResults[i];

            if (!hit.collider.TryGetComponent(out Renderer hitRenderer))
            {
                continue;   
            }
             //Debug.Log("FoundRenderer");
            //すでに確認した障害物か
            if (entries.TryGetValue(hitRenderer, out Entry entry))
            {
                entry.wasHitThisFrame = true;
                continue;
            }
            
            Entry newEntry = new Entry
            {
                wasHitThisFrame = true,
                originalShadow = hitRenderer.shadowCastingMode   //戻す時のために保存
            };
            entries.Add(hitRenderer, newEntry);
            
            hitRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;

        }
    }

    private void UpdateFade()
    {

        removeBuffer.Clear();
        
        foreach(var e in entries)
        {
            Renderer renderer = e.Key;
            if (e.Value.wasHitThisFrame)
            {
                continue;
            }

            if(renderer == null)
            {
                removeBuffer.Add(renderer);
                continue;
            }

            renderer.shadowCastingMode = e.Value.originalShadow;

            removeBuffer.Add(renderer);
        }

        for(int i = 0; i < removeBuffer.Count; i++)
        {
            entries.Remove(removeBuffer[i]);
        }
    }

    //全てを元に戻す。CameraControllerのOnDisable()で実行
    public void Clear()
    {
        foreach(var e in entries)
        {
            if(e.Key == null){ continue;}
            e.Key.shadowCastingMode = e.Value.originalShadow;
        }
        entries.Clear();
    }
    
    private Vector3 GetTargetPoint()
    {
        if(target==null){ return Vector3.zero;}

        return new Vector3(target.position.x, target.position.y + 1.2f, target.position.z);
    }
}
