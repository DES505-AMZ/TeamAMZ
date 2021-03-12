using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemInfoCanvas : MonoBehaviour
{
    public Text itemName;
    public Text itemInfo;
    public Image itemImage;

    public void ShowItemInfo(EvidenceItem item)
    {
        gameObject.SetActive(true);

        itemName.text = item.itemName;
        itemInfo.text = item.itemInfo;
        itemImage.sprite = item.itemImage;

        GameManager.Instance.PauseGame();
    }

    public void OnButtonClick()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}
