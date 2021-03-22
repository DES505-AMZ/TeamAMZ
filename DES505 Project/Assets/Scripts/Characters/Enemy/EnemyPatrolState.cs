using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyStateBase
{
    float navInterval = 0.6f;
    float timeCounter = 0f;

    public override void Enter(EnemyBase enemy)
    {
        enemy.UpdateMaxSpeed(enemy.patrolSpeed);
    }

    public override void Exit(EnemyBase enemy)
    {
    }

    public override void UpdateLogic(EnemyBase enemy)
    {
    }

    public override void UpdatePhysics(EnemyBase enemy)
    {
        timeCounter += Time.fixedDeltaTime;
        if (timeCounter >= navInterval)
        {
            enemy.UpdateTargetLocation(enemy.target.transform.position);
            timeCounter = 0f;
        }
        enemy.LookOrientTowards(enemy.target.transform.position);

        if (enemy.SightDetection())
        {
            enemy.ChangeState(enemy.chaseState);
        }

        if(enemy.SoundDetection())
        {
            enemy.ChangeState(enemy.seekState);
        }
    }
}
