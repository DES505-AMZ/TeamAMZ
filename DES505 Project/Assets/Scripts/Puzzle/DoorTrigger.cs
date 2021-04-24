using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : Interactable
{
    public bool canPickup = true;
    public string promptTextKey = "Collect Key";
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
        {
            gameObject.SetActive(false);
            UIManager.Instance.ShowPromptCanvas(false);
        }
    }

    public override void OnLookAt()
    {
        if (canPickup)
            UIManager.Instance.ShowPromptCanvas(true, promptTextKey);
        else
            UIManager.Instance.ShowPromptCanvas(true);
    }

    public override void OnLookExit()
    {
        UIManager.Instance.ShowPromptCanvas(false);
    }
}
