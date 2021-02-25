﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorElectronicSwitch : DoorBase
{
    [Tooltip("Switch object (only works when HasTrigger set to true)")]
    public DoorTrigger switchObject;

    private void Start()
    {
        switchObject.onDoor += DoorOpen;
    }

    protected override bool CanOpenByPlayer()
    {
        if (!hasTrigger || (hasTrigger && switchObject.isActivated))
            return true;
        else
            return false;
    }

    protected override void DoorOpen()
    {
        base.DoorOpen();
        switchObject.onDoor -= DoorOpen;
        switchObject.onDoor += DoorClose;
    }

    protected override void DoorClose()
    {
        base.DoorClose();
        switchObject.onDoor -= DoorClose;
        switchObject.onDoor += DoorOpen;
    }
}
