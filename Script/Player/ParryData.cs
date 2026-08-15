using UnityEngine;

[CreateAssetMenu(fileName = "ParryData", menuName = "Scriptable Objects/ParryData")]
public class ParryData : ScriptableObject
{
    [Header("パリータイミング")]
    public float windowStart = 0.05f;
    public float windowEnd =0.2f;
    public float stateEnd = 0.5f;
}
