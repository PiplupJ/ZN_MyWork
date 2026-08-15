using UnityEngine;

public abstract class GolemBaseState : EnemyState

{
    protected GolemStateMachine stateMachine;

    protected GolemBaseState(GolemStateMachine stateMachine)
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

    //プレイヤーが敵の監視範囲以内かを判定
    protected bool InChaseRange()
    {
        return DistToPlayer()<= stateMachine.ChasingRange;
    }

    //プレイヤーが敵の攻撃範囲以内かを判定
    protected bool InLaserAttackRange()
    {
        
        return DistToPlayer()<= stateMachine.LaserAttackRange;
    }

    protected bool InMeleeAttackRange()
    {   
        return DistToPlayer()<= stateMachine.MeleeAttackRange;
    }
    

    protected void GolemMove(float deltaTime, float moveSpeed, Vector3 dir)
    {   
        if (stateMachine.GolemController == null || !stateMachine.GolemController.enabled)
        {
            Debug.LogError("No CharacterController");
            return;
        }

        Vector3 movementVector = dir * moveSpeed * deltaTime;
        
        stateMachine.GolemController.Move(movementVector);
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

    protected void MoveAndRotateToPlayer(float deltaTime, float moveSpeed)
    {
        RotateToPlayer(deltaTime);
        if(TryGetDirToPlayer(out Vector3 dir)){
            GolemMove(deltaTime, moveSpeed, dir);    
        }
                                      
    }

}
