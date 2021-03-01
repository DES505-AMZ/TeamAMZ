using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    public CollectableItem[] allItems;
    [SerializeField]
    public List<CollectableItem> foundItems;
    public Canvas itemCanvas;
    public Text itemName;
    public Text itemInfo;
    public Image itemImage;

    private void Awake()
    {
        if(allItems.Length == 0)
        {
            allItems = FindObjectsOfType<CollectableItem>();
        }
    }

    void Start()
    {
        foreach(CollectableItem item in allItems)
        {
            item.onFound += OnFound;
        }
        itemCanvas.gameObject.SetActive(false);
    }

    void OnFound(CollectableItem item)
    {
        item.onFound -= OnFound;
        foundItems.Add(item);
        ShowItemInfo(item.info);
    }

    void ShowItemInfo(EvidenceItem item)
    {
        Debug.Log(item.itemName);
        itemCanvas.gameObject.SetActive(true);
        itemName.text = item.itemName;
        itemInfo.text = item.itemInfo;
        itemImage.sprite = item.itemImage;
        GameManager.PauseGame();
    }

    public void OnGetButton()
    {
        itemCanvas.gameObject.SetActive(false);
        GameManager.ResumeGame();
    }
}
