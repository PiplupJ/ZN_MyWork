using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class KnockbackEntry
{
    public AttackType type;
    public float initialSpeed;
    public float deceleration;
    public float cancelNormalizedTime;
    public string motionTag;
}

[CreateAssetMenu(fileName = "KnockbackTable", menuName = "Scriptable Objects/KnockbackTable")]
public class KnockbackTable : ScriptableObject
{
    [SerializeField] 
    private List<KnockbackEntry> entries = new List<KnockbackEntry>();

    public bool TryGetEntry(AttackType type, out KnockbackEntry entry)
    {
        entry = null;

        foreach(var e in entries)
        {
            if(e.type == type)
            {
                entry = e;
                return true;
            }
        }

        return false;
    }
}
