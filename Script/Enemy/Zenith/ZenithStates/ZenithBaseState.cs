using UnityEngine;

public abstract class ZenithBaseState : EnemyState

{
     protected ZenithStateMachine stateMachine;

    protected ZenithBaseState(ZenithStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    //プレイヤーが敵の監視範囲以内かを判定
    protected bool InChaseRange()
    {
        //シュウが追加しました
        if (stateMachine.Player == null)
        {
            Debug.Log("Cannot Find Player");
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }

        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= stateMachine.ChasingRange*stateMachine.ChasingRange;
    }

    //プレイヤーが敵の攻撃範囲以内かを判定
    protected bool InMeleeAttackRange()
    {
        if (stateMachine.Player == null)
        {
            Debug.Log("Cannot Find Player");
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }
        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= stateMachine.MeleeAttackRange*stateMachine.MeleeAttackRange;
    }
    
    protected bool InRangedAttackRange()
    {
        if (stateMachine.Player == null)
        {
            Debug.Log("Cannot Find Player");
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }
        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= stateMachine.RangedAttackRange*stateMachine.RangedAttackRange;
    }

    protected bool InEscapeRange()
    {
        if (stateMachine.Player == null)
        {
            Debug.Log("Cannot Find Player");
            stateMachine.Player = GameObject.FindGameObjectWithTag("Player");
            return false;
        }
        float distSqr = 
        (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;
        
        return distSqr<= stateMachine.EscapeRange*stateMachine.EscapeRange;
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

    protected void MoveToPlayer(float deltaTime, float moveSpeed)
    {   
        if (stateMachine.zenithController == null || !stateMachine.zenithController.enabled)
        {
            Debug.LogError("No CharacterController");
            return;
        }

        if(stateMachine.Player==null){return;}

        Vector3 curPos = stateMachine.zenithController.transform.position; 
        
        Vector3 dir = (stateMachine.Player.transform.position - curPos).normalized;

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

        Vector3 curPos = stateMachine.zenithController.transform.position; 
        
        Vector3 dir = -((stateMachine.Player.transform.position - curPos).normalized);

        Vector3 movementVector = dir * moveSpeed * deltaTime;
        
        stateMachine.zenithController.Move(movementVector);
    }
}
