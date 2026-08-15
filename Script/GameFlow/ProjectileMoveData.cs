using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileMoveData", menuName = "Scriptable Objects/ProjectileMoveData")]
public class ProjectileMoveData : ScriptableObject
{
    public float baseSpeed;
    public float maxSpeed;
    public float acceleration;
    public float lifetime;
}
