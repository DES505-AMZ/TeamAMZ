using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerStateBase
{
    public override void Enter(Player player)
    {
        //Debug.Log("run enter");
        player.targetMoveSpeed = player.maxWalkSpeed * player.runSpeedRatio;
        player.maxNoiseRange = player.noiseRange * player.runNoiseRatio;
    }

    public override void Exit(Player player)
    {
        //Debug.Log("run exit");
    }

    public override void UpdateLogic(Player player)
    {

    }

    public override void UpdatePhysics(Player player)
    {
        player.HandleMovement();

        if (player.inputHandler.GetRunInputReleased())
        {
            player.ChangeStateMovement(player.walkState);
        }
    }
}
