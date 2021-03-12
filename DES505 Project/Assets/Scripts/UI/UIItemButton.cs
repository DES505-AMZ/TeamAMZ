using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemButton : MonoBehaviour
{
    public EvidenceItem itemInfo;
    public Text text;

    public void SetItem(EvidenceItem item)
    {
        itemInfo = item;
        text.text = item.name;
    }

    public void OnButtonClick()
    {
        if(itemInfo != null)
            UIManager.Instance.ShowItemInfoCanvas(itemInfo);
    }
}
