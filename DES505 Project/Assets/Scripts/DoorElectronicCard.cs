using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorElectronicCard : DoorBase
{
    public DoorTrigger leftCard;
    public DoorTrigger rightCard;

    protected override bool CanOpenByPlayer()
    {
        if (!hasTrigger || (hasTrigger && leftCard.isActivated && rightCard.isActivated))
            return true;
        else
            return false;
    }
}
