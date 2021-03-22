using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerStateBase
{
    public override void Enter(Player player)
    {
        //Debug.Log("walk enter");
        player.targetMoveSpeed = player.maxWalkSpeed;
        player.characterHeight = player.capsuleHeightStanding;
        player.maxNoiseRange = player.noiseRange;
    }

    public override void Exit(Player player)
    {
        //Debug.Log("walk exit");
    }

    public override void UpdateLogic(Player player)
    {
        player.UpdateCharacterHeight(true);
    }

    public override void UpdatePhysics(Player player)
    {
        player.HandleMovement();

        if(player.inputHandler.GetCrouchInputDown())
        {
            player.ChangeStateMovement(player.crouchState);
        }
        else if(player.inputHandler.GetRunInputDown())
        {
            player.ChangeStateMovement(player.runState);
        }
    }
}
