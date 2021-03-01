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
    }

    void OnFound(CollectableItem item)
    {
        item.onFound -= OnFound;
        foundItems.Add(item);
    }
}
