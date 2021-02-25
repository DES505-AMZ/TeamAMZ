using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNormal : DoorBase
{
    [Tooltip("Key object (only works when HasTrigger set to true)")]
    public DoorTrigger key;
    [Tooltip("Is the door a one-way door?")]
    public bool isOneWayDoor = false;
    [Tooltip("The direction of one-way door's front side. Set to +z axis by default.")]
    public Transform forwardPoint;

    Vector3 forward;

    private void Start()
    {
        if (forwardPoint == null)
            forward = transform.forward;
        else
        {
            forward = Vector3.ProjectOnPlane(forwardPoint.position - transform.position, Vector3.up);
            Vector3.Normalize(forward);
        }
    }

    protected override bool CanOpenByPlayer()
    {
        if (!hasTrigger || (hasTrigger && key.isActivated))
        {
            if (isOneWayDoor && Vector3.Dot(m_playerViewRay.direction, forward) > 0)
                return false;
            return true;
        }
        else
            return false;
    }

    //protected override void DoorOpen()
    //{
    //    m_isOpen = true;
    //}

    //protected override void DoorClose()
    //{
    //    m_isOpen = false;
    //}
}
