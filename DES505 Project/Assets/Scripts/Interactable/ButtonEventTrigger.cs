using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventTrigger : Interactable
{
    public TriggerEvent[] eventObjects;
    bool isInteracted = false;

    public override void OnInteraction(Ray ray)
    {
        base.OnInteraction(ray);
        if (!isInteracted)
        {
            isInteracted = true;
            foreach (var obj in eventObjects)
            {
                obj.OnEvent();
            }
        }
    }

    public override void OnLookAt()
    {
        if (!isInteracted)
            UIManager.Instance.ShowPromptCanvas(true);
    }

    public override void OnLookExit()
    {
        if (!isInteracted)
            UIManager.Instance.ShowPromptCanvas(false);
    }
}
