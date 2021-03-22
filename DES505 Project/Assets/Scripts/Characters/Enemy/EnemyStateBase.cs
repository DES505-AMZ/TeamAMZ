using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase
{
    public virtual void Enter(EnemyBase enemy) { }
    public virtual void UpdateLogic(EnemyBase enemy) { }
    public virtual void UpdatePhysics(EnemyBase enemy) { }
    public virtual void Exit(EnemyBase enemy) { }
}
