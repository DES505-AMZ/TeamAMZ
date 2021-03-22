using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_patrol : EnemyBase
{
    protected override void InitializeState()
    {
        patrolState = new EnemyPatrolState_patrol();
        seekState = new EnemySeekState_patrol();
        chaseState = new EnemyChaseState_patrol();
    }
}
