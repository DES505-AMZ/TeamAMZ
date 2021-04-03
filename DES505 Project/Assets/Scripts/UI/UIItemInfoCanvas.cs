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
