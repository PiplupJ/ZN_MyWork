using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundDB", menuName = "Scriptable Objects/SoundDB")]
public class SoundDB : ScriptableObject
{
    public List<SoundData> clips;
}
