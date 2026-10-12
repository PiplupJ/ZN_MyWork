//ダッシュエフェクト。プレイヤーに直接アタッチされる想定。TrailRendererとParticleSystemを使う
using UnityEngine;

public class DashFX : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] trails;　//向きを見せる。事前に設定されたTrailRendererを配列で持つ
    [SerializeField] private ParticleSystem dashParticles; //ダッシュの煙

    //引数として移動方向を受け取り、パーティクルの向きを設定して再生する
    public void Begin(Vector3 dir)
    {
        //移動向きの反対側
        dashParticles.transform.rotation = Quaternion.LookRotation(-dir);
        dashParticles.Play();

        foreach(var trail in trails)
        {
            trail.Clear();
            trail.emitting = true;
        }
    }

    public void End()
    {
        dashParticles.Stop();
        foreach (var trail in trails)
        {
            trail.emitting = false;
        }
    }
}
