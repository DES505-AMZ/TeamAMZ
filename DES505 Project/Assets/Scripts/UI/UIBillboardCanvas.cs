using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBillboardCanvas : UICanvas
{
    public Image image;
    public Sprite sprite;

    public override void ActivateCanvas()
    {
        if (sprite != null)
        {
            image.sprite = sprite;
            float aspectRatio = sprite.rect.width / sprite.rect.height;
            float h = Screen.height * 0.8f;
            float w = h * aspectRatio;
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        }

        base.ActivateCanvas();
    }

    void Update()
    {
        if(isActive)
        {
            if (Input.GetButtonDown(GameConstants.k_ButtonNameAim))
            {
                DeactivateCanvas();
            }
        }
    }
}
