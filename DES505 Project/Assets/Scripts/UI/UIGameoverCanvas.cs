using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameoverCanvas : UICanvas
{
    public override void DeactivateCanvas()
    {
        base.DeactivateCanvas();
        if (UIManager.Instance.onButtonBackToCheckPoint != null)
            UIManager.Instance.onButtonBackToCheckPoint();
    }

    private void Update()
    {
        if (isActive)
        {
            if(Input.anyKeyDown)
            {
                DeactivateCanvas();
            }
        }
    }
}
