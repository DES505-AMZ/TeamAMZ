using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemCollectCanvas : UICanvas
{
    public override void ActivateCanvas()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public override void DeactivateCanvas()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
