using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtState : PlayerStateBase
{
    public Vector3 caughtPosition;
    Quaternion targetRotation;
    bool isOnCaught = false;
    public override void Enter(Player player)
    {
        isOnCaught = false;
        player.inputHandler.canProcessInput = false;
        targetRotation = Quaternion.LookRotation(caughtPosition - player.transform.position);

        player.tannoy.PlayGameOver();
    }

    public override void Exit(Player player)
    {
        player.inputHandler.canProcessInput = true;
    }

    public override void UpdateLogic(Player player)
    {

    }

    public override void UpdatePhysics(Player player)
    {
        player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, 3);
        if (!isOnCaught && player.transform.rotation == targetRotation)
        {
            isOnCaught = true;
            if (player.onCaught != null)
                player.onCaught();
        }
    }
}
