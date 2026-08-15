using UnityEngine;

public abstract class ZenithBaseState : EnemyState

{
    protected ZenithStateMachine stateMachine;

    protected ZenithBaseState(ZenithStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected float DistToPlayer()
    {
        if(stateMachine.Player == null){
            return float.MaxValue;
        }
        return Vector3.Distance(
            stateMachine.Player.transform.position,
            stateMachine.transform.position);
    }

    protected bool TryGetDirToPlayer(out Vector3 dir)
    {
        dir = Vector3.zero;
        if (stateMachine.Player == null) { return false; }

        Vector3 diff = stateMachine.Player.transform.position
                    - stateMachine.transform.position;
        diff.y = 0f;

        if (diff.sqrMagnitude < 0.0001f) { return false; }

        dir = diff.normalized;
        return true;
    }    

    protected bool TryGetPlayerPos(out Vector3 playerPos)
    {
        playerPos = default;  
        if (stateMachine.Player == null) { return false; }

        playerPos = stateMachine.Player.transform.position;
        return true;
    }

    //プレイヤーが敵の監視範囲以内かを判定
    protected bool InChaseRange()
    {   
        return DistToPlayer()<= stateMachine.ChasingRange;
    }

    protected bool InTargetRange()
    {
        return DistToPlayer() <= stateMachine.targetRange;
    }

    protected void RotateToPlayer(float deltaTime)
	{
        if (!TryGetDirToPlayer(out Vector3 dir)) { return; }
  
        Quaternion targetRot = Quaternion.LookRotation(dir);
        stateMachine.transform.rotation = Quaternion.RotateTowards(
        stateMachine.transform.rotation,
        targetRot,
        stateMachine.rotateSpeed * deltaTime);
    }

    protected void MoveToPlayer(float deltaTime, float moveSpeed)
    {   
        if (stateMachine.zenithController == null || !stateMachine.zenithController.enabled)
        {
            Debug.LogError("No CharacterController");
            return;
        }

        if (!TryGetDirToPlayer(out Vector3 dir)) { return; }

        Vector3 movementVector = dir * moveSpeed * deltaTime;
        
        stateMachine.zenithController.Move(movementVector);
    }

    protected void EscapeFromPlayer(float deltaTime, float moveSpeed)
    {
        if (stateMachine.zenithController == null || !stateMachine.zenithController.enabled)
        {
            Debug.LogError("No CharacterController");
            return;
        }
        
        if(stateMachine.Player==null){return;}

        if (!TryGetDirToPlayer(out Vector3 dir)) { return; }

        Vector3 movementVector = -dir * moveSpeed * deltaTime;
        
        stateMachine.zenithController.Move(movementVector);
    }
    protected bool OnSight()
    {
        if(!TryGetDirToPlayer(out Vector3 dir)){
            return false;
        }

        Vector3 fwd = stateMachine.transform.forward;
        fwd.y = 0;

        float angle = Vector3.Angle(fwd, dir);

        return angle < stateMachine.sightAngle;
    }

    protected void MoveWithAccel(float deltaTime, float targetSpeed, Vector3 dir)
    {
        Vector3 targetVelocity = dir * targetSpeed;

        //加速
        if(targetVelocity.sqrMagnitude > stateMachine.currentVelocity.sqrMagnitude){
            stateMachine.currentVelocity = Vector3.MoveTowards(stateMachine.currentVelocity, targetVelocity, stateMachine.moveAcceleration * deltaTime);
        }
        //減速
        else{
            stateMachine.currentVelocity = Vector3.MoveTowards(stateMachine.currentVelocity, targetVelocity, stateMachine.moveDeceleration*deltaTime);
        }
        Vector3 movementVector = stateMachine.currentVelocity * deltaTime;

        stateMachine.zenithController.Move(movementVector);
    }

    protected void MoveWithFixedSpeed(float deltaTime, float moveSpeed, Vector3 dir)
    {
        Vector3 movementVector = dir * moveSpeed * deltaTime;
        
        stateMachine.zenithController.Move(movementVector);
    }

}
