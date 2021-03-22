using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeekState : EnemyStateBase
{
    public override void Enter(EnemyBase enemy)
    {
        enemy.UpdateMaxSpeed(enemy.seekSpeed);
    }

    public override void Exit(EnemyBase enemy)
    {
    }

    public override void UpdateLogic(EnemyBase enemy)
    {
    }

    public override void UpdatePhysics(EnemyBase enemy)
    {
        enemy.UpdateTargetLocation(enemy.targetLocationLastSeen);
        enemy.LookOrientTowards(enemy.target.transform.position);

        if (enemy.SightDetection())
        {
            enemy.ChangeState(enemy.chaseState);
        }

        if(!enemy.SoundDetection())
        {
            enemy.ChangeState(enemy.patrolState);
        }
    }
}
