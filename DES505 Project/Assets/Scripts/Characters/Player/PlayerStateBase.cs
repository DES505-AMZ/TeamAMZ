using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase
{
    public virtual void Enter(Player player) { }
    public virtual void UpdateLogic(Player player) { }
    public virtual void UpdatePhysics(Player player) { }
    public virtual void Exit(Player player) { }
}
