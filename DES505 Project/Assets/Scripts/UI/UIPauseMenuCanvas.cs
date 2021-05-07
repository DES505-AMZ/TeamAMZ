using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenuCanvas : UICanvas
{
    public void OnResume()
    {
        base.DeactivateCanvas();
    }

    public void OnExit()
    {
        GameManager.Instance.RestartGame();
    }
}
