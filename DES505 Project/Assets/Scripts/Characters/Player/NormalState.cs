using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : PlayerStateBase
{
    public override void Enter(Player player)
    {
        player.currentRotationSpeed = player.rotationSpeed;
    }

    public override void Exit(Player player)
    {

    }

    public override void UpdateLogic(Player player)
    {
        player.HandleLookRotation();
        if (player.GetInteractableObject())
        {
            player.InteractWithObject();
        }

        if (player.inputHandler.GetAimInputDown())
        {
            player.ChangeStateAction(player.photoState);
        }
    }

    public override void UpdatePhysics(Player player)
    {
        
    }
}
