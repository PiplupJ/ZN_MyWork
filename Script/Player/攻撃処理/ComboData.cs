using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ComboData", menuName = "Scriptable Objects/ComboData")]
public class ComboData : ScriptableObject
{
    public List<AttackData> combo = new List<AttackData>();
}
