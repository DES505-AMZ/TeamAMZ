using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    protected bool isActive = false;

    public virtual void ActivateCanvas()
    {
        isActive = true;
        gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    public virtual void DeactivateCanvas()
    {
        isActive = false;
        gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}
