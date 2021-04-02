using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : PlayerStateBase
{
    public override void Enter(Player player)
    {
        //Debug.Log("crouch enter");
        player.targetMoveSpeed = player.maxWalkSpeed * player.crouchedSpeedRatio;
        player.characterHeight = player.capsuleHeightCrouching;
        player.maxNoiseRange = player.noiseRange * player.crouchedNoiseRatio;
    }

    public override void Exit(Player player)
    {
        //Debug.Log("crouch exit");
    }

    public override void UpdateLogic(Player player)
    {
        player.UpdateCharacterHeight(true);

        if (player.inputHandler.GetCrouchInputDown())
        {
            player.ChangeStateMovement(player.walkState);
        }
    }

    public override void UpdatePhysics(Player player)
    {
        player.HandleMovement();       
    }
}
