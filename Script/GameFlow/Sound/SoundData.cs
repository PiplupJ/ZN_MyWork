using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    public SoundType type;
    public AudioClip clip;
    public float pitch = 0;
}
