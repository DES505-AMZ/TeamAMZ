using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : Interactable
{
    public bool canPickup = true;
    public bool isActivated { get; private set; }

    public UnityAction onDoor;

    private void Start()
    {
        isActivated = false;
    }

    public override void OnInteraction(Ray ray)
    {
        base.OnInteraction(ray);

        if (!isActivated)
        {
            isActivated = true;
        }

        // turn on the switch will tell the door to open
        if (onDoor != null)
            onDoor();

        if (canPickup)
            gameObject.SetActive(false);
    }
}
