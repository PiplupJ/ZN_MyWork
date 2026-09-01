using UnityEngine;

[CreateAssetMenu(fileName = "RocketMoveData", menuName = "Scriptable Objects/RocketMoveData")]
public class RocketMoveData : ProjectileMoveData
{
    public float risetime = 2f;
    public float turnSpeed = 180f;

    public override Vector3 GetStartVelocity(Vector3 pos, Vector3 targetPos)
    {
        return Vector3.up*baseSpeed;
    }

    public override Vector3 GetNextVelocity(
        Vector3 currentVelocity, Vector3 currentPos, Vector3 targetPos,
        float moveSpeed, float elapsed, float dt)
    {
        if (elapsed < risetime)
            return currentVelocity.normalized * moveSpeed*dt; //上昇区間

        Vector3 desired = (targetPos - currentPos).normalized * moveSpeed*dt;
        return Vector3.RotateTowards(
            currentVelocity, desired,
            turnSpeed * Mathf.Deg2Rad * dt,
            moveSpeed * dt);              
    }    

}
