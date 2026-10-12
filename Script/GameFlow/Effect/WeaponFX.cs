//武器を振り回さす際のエフェクト制御用クラス
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] trails;

    public void Begin()
    {

        foreach (var trail in trails)
        {
            trail.Clear();
            trail.emitting = true;
        }
    }

    public void End()
    {
        foreach (var trail in trails)
        {
            trail.emitting = false;
        }
    }
}
