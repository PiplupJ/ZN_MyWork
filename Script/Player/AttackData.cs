using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct AttackProfile
{
    public string name;
    public string motionTag;
    public float activeFrame;
    public float recoverFrame;
}
[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public List<AttackProfile> attackDatas;
}
