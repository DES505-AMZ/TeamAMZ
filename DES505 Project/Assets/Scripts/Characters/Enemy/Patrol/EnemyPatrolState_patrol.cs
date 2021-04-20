using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState_patrol : EnemyPatrolState
{
    public override void Enter(EnemyBase enemy)
    {
        base.Enter(enemy);
        enemy.navAgent.SetPathDestinationToClosestNode();
    }

    public override void UpdatePhysics(EnemyBase enemy)
    {
        enemy.navAgent.UpdatePathDestination();
        if (enemy.navAgent.DestinationArrived())
        {
            Vector3 dest = enemy.navAgent.GetPatrolPathDestination();
            if(dest != Vector3.zero)
                enemy.navAgent.SetNavDestination(dest);
        }

        if (enemy.SightDetection())
        {
            if(enemy.clipCloseTargetUnnoticed != null)
                enemy.audioSource.PlayOneShot(enemy.clipCloseTargetUnnoticed);
            enemy.ChangeState(enemy.chaseState);
        }

        if (enemy.SoundDetection())
        {
            enemy.ChangeState(enemy.seekState);
        }
    }
}
