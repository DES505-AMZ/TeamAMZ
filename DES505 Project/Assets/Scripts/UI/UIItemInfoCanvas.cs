using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemInfoCanvas : UICanvas
{
    public Text itemName;
    public Text itemInfo;
    public Image itemImage;
    public EvidenceItem item;

    public override void ActivateCanvas()
    {
        itemName.text = item.itemName;
        itemInfo.text = item.itemInfo;
        itemImage.sprite = item.itemImage;

        float aspectRatio = item.itemImage.rect.width / item.itemImage.rect.height;
        float h = itemImage.rectTransform.rect.height;
        float w = h * aspectRatio;
        itemImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);

        base.ActivateCanvas();
    }

    private void Update()
    {
        if (isActive)
        {
            if (Input.GetButtonDown(GameConstants.k_ButtonNameAim))
            {
                DeactivateCanvas();
            }
        }
    }
}
