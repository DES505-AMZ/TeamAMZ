using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    bool isStun = false;
    float stunTimer = 0f;

    public override void Enter(EnemyBase enemy)
    {
        enemy.UpdateMaxSpeed(enemy.chaseSpeed);
        isStun = false;
        enemy.target.onPhoto += OnStun;
    }

    public override void Exit(EnemyBase enemy)
    {
        if(enemy.clipLostTarget != null)
            enemy.audioSource.PlayOneShot(enemy.clipLostTarget);
    }

    public override void UpdateLogic(EnemyBase enemy)
    {
        if (isStun)
        {
            enemy.UpdateMaxSpeed(0);
            stunTimer += Time.deltaTime;
            if (stunTimer >= enemy.stunTimeInterval)
            {
                isStun = false;
                stunTimer = 0f;
                enemy.UpdateMaxSpeed(enemy.chaseSpeed);
            }
        }
    }

    public override void UpdatePhysics(EnemyBase enemy)
    {
        if (!isStun)
        {
            enemy.UpdateTargetLocation(enemy.target.transform.position);
            enemy.LookOrientTowards(enemy.target.transform.position);

            if (enemy.GetDistanceToTarget() > enemy.sightRange)
            {
                enemy.targetLocationLastSeen = enemy.target.transform.position;
                enemy.ChangeState(enemy.seekState);
            }
        }
    }

    void OnStun(Ray ray)
    {
        isStun = true;
        stunTimer = 0f;
    }
}
