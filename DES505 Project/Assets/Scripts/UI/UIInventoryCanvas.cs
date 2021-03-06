﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryCanvas : MonoBehaviour
{
    public Dictionary<GameConstants.LevelArea, UIInventoryPanel> panels = new Dictionary<GameConstants.LevelArea, UIInventoryPanel>();
    //public List<UIInventoryPanel> panelList = new List<UIInventoryPanel>();

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
}
