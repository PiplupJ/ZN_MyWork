using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectDataBase", menuName = "Scriptable Objects/EffectDataBase")]
public class EffectDataBase : ScriptableObject
{
    public List<EffectData> effects = new();
}
