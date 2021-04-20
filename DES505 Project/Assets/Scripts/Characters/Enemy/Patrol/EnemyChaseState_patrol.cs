using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState_patrol : EnemyChaseState
{
    public override void Enter(EnemyBase enemy)
    {
        base.Enter(enemy);
        if(enemy.clipsDetectTarget != null)
            enemy.audioSource.PlayOneShot(enemy.clipsDetectTarget[Random.Range(0, enemy.clipsDetectTarget.Length - 1)]);
    }

    public override void UpdatePhysics(EnemyBase enemy)
    {
        enemy.UpdateTargetLocation(enemy.target.transform.position);
        enemy.LookOrientTowards(enemy.target.transform.position);

        if (!enemy.SightDetection())
        {
            enemy.targetLocationLastSeen = enemy.target.transform.position;
            enemy.ChangeState(enemy.seekState);
        }
    }
}
