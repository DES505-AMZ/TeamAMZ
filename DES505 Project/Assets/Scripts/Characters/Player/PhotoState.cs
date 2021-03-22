using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoState : PlayerStateBase
{
    public override void Enter(Player player)
    {
        player.currentRotationSpeed = player.rotationSpeed * player.photoingRotationMultiplier;
    }

    public override void Exit(Player player)
    {

    }

    public override void UpdateLogic(Player player)
    {
        UIManager.Instance.SetCameraCanvasVisible(true);
        player.HandleLookRotation();
        player.TakePhoto();

        if (player.inputHandler.GetAimInputReleased())
        {
            UIManager.Instance.SetCameraCanvasVisible(false);
            player.ChangeStateAction(player.normalState);
        }
    }

    public override void UpdatePhysics(Player player)
    {

    }
}
