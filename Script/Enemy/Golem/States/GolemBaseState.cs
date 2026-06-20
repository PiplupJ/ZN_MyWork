using UnityEngine;

public abstract class GolemBaseState : EnemyState

{
    protected GolemStateMachine stateMachine;

    protected GolemBaseState(GolemStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    //プレイヤーが敵の監視範囲以内かを判定
    protected bool InChaseRange()
    {
        //シュウが追加しました
        if (stateMachine.Player == null)
        {
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }

        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= stateMachine.ChasingRange*stateMachine.ChasingRange;
    }

    //プレイヤーが敵の攻撃範囲以内かを判定
    protected bool InLaserAttackRange()
    {
        if (stateMachine.Player == null)
        {
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }

        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= stateMachine.LaserAttackRange*stateMachine.LaserAttackRange ;
    }

    protected bool InMeleeAttackRange()
    {
        if (stateMachine.Player == null)
        {
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }

        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= stateMachine.MeleeAttackRange*stateMachine.MeleeAttackRange ;
    }
    

    protected void GolemMove(float deltaTime, float moveSpeed, Vector3 goal)
    {   
        if (stateMachine.GolemController == null || !stateMachine.GolemController.enabled)
        {
            Debug.LogError("No CharacterController");
            return;
        }
    
        Vector3 curPos = stateMachine.GolemController.transform.position; 
        
        Vector3 dir = (goal - curPos).normalized;

        Vector3 movementVector = dir * moveSpeed * deltaTime;
        
        stateMachine.GolemController.Move(movementVector);
    }

    protected void GolemMoveEscape(float deltaTime, float moveSpeed, Vector3 goal)
    {
        if (stateMachine.GolemController == null || !stateMachine.GolemController.enabled)
        {
            Debug.LogError("No CharacterController");
            return;
        }
        
        Vector3 curPos = stateMachine.GolemController.transform.position; 
        
        Vector3 dir = ((curPos - goal).normalized);

        Vector3 movementVector = dir * moveSpeed * deltaTime;
        
        stateMachine.GolemController.Move(movementVector);

    }

    protected void RotateToPlayer(float deltaTime)
	{
        if(stateMachine.Player == null) { return; }
        Vector3 dir = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        dir.y = 0f;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        stateMachine.transform.rotation = Quaternion.RotateTowards(
        stateMachine.transform.rotation,
        targetRot,
        stateMachine.rotateSpeed * deltaTime);
    }

    protected void MoveAndRotateToPlayer(float deltaTime, float moveSpeed)
    {
        if (stateMachine.Player == null) { return; }

        Vector3 playerPos = stateMachine.Player.transform.position;

        Vector3 dir = playerPos - stateMachine.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            stateMachine.transform.rotation = Quaternion.RotateTowards(
                stateMachine.transform.rotation, targetRot,
                stateMachine.rotateSpeed * deltaTime);
        }

        GolemMove(deltaTime, moveSpeed, playerPos);                                  
    }

}
