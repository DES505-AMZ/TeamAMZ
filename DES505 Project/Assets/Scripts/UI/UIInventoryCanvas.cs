using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryCanvas : UICanvas
{
    public Dictionary<GameConstants.LevelArea, UIInventoryPanel> panels = new Dictionary<GameConstants.LevelArea, UIInventoryPanel>();

    void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            UIInventoryPanel panel = transform.GetChild(i).GetComponent<UIInventoryPanel>();
            if (panel != null)
            {
                panels.Add(panel.level, panel);
            }
        }
    }

    public void UpdatePhotoInfo(GameConstants.LevelArea levelArea)
    {
        panels[levelArea].OnFoundPhoto();
    }

    public void UpdateItemInfo(GameConstants.LevelArea levelArea, EvidenceItem item)
    {
        panels[levelArea].OnFoundItem(item);
    }

    private void Update()
    {
        if (isActive)
        {
            if (Input.GetButtonDown(GameConstants.k_ButtonNameInventory) || Input.GetButtonDown(GameConstants.k_ButtonNameAim))
            {
                //if (gameObject.activeInHierarchy)
                DeactivateCanvas();
            }
        }
    }
}
