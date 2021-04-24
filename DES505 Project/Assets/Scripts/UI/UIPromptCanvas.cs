using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPromptCanvas : UICanvas
{
    public Text text;

    public void UpdateText(string str)
    {
        text.text = str;
    }

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
