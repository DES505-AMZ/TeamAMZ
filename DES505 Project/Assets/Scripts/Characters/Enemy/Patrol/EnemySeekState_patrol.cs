using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeekState_patrol : EnemySeekState
{
    bool firstSeek = false;

    public override void Enter(EnemyBase enemy)
    {
        base.Enter(enemy);
        firstSeek = false;
    }

    public override void UpdatePhysics(EnemyBase enemy)
    {
        if(!firstSeek && enemy.GetDistanceToTarget() <= enemy.sightRange)
        {
            enemy.audioSource.PlayOneShot(enemy.clipCloseTargetUnnoticed);
            firstSeek = true;
        }

        enemy.UpdateTargetLocation(enemy.targetLocationLastSeen);
        enemy.LookOrientTowards(enemy.targetLocationLastSeen);

        if (enemy.SightDetection())
        {
            enemy.ChangeState(enemy.chaseState);
        }

        if (!enemy.SoundDetection())
        {
            enemy.ChangeState(enemy.patrolState);
        }
    }
}
