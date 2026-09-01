using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileMoveData", menuName = "Scriptable Objects/ProjectileMoveData")]
public class ProjectileMoveData : ScriptableObject
{
    public float baseSpeed;
    public float maxSpeed;
    public float acceleration;
    public float lifetime;
    
    public bool horizontalOnly;

    //発射体の初期速度ベクトルを求める
    public virtual Vector3 GetStartVelocity(Vector3 pos, Vector3 targetPos)
    {
        Vector3 diff = targetPos - pos;
        if (horizontalOnly)
        {
            diff.y = 0f;
        }


        return diff.normalized * baseSpeed;
    }

    //現在フレームの移動ベクトルを求める
    public virtual Vector3 GetNextVelocity(
        Vector3 currentVelocity, Vector3 currentPos, Vector3 targetPos,
        float moveSpeed, float elapsed, float dt)
    {
        return currentVelocity.normalized * moveSpeed*dt;
    }
}
