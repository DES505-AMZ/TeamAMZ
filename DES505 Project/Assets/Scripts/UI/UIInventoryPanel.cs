using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryPanel : MonoBehaviour
{
    public GameConstants.LevelArea level;
    public Text areaNameText;
    public Text photoInfoText;
    public Image photoCheckImage;
    public UIItemButton[] itemButtons;
    public Sprite photoCheckSprite;
    public Sprite photoUncheckSprite;

    int nextButtonIndex = 0;

    private void Start()
    {
        photoCheckImage.sprite = photoUncheckSprite;
    }

    public void OnFoundPhoto()
    {
        photoCheckImage.sprite = photoCheckSprite;
    }

    public void OnFoundItem(EvidenceItem item)
    {
        if (nextButtonIndex < itemButtons.Length)
            itemButtons[nextButtonIndex++].SetItem(item);
    }
}
