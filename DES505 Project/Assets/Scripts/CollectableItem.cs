using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate void CollectableAction(EvidenceItem item);
public class CollectableItem : Interactable
{
    bool isLookingAt = false;
    public bool isFound { get; private set; }

    //public event CollectableAction onFound;

    public GameConstants.LevelArea level;
    public EvidenceItem info;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if (isLookingAt)
            UIManager.Instance.ShowItemCollectCanvas(true);
        else
            UIManager.Instance.ShowItemCollectCanvas(false);
        isLookingAt = false;
    }

    public override void OnInteraction(Ray ray)
    {
        base.OnInteraction(ray);

        isFound = true;
        //if (onFound != null)
        //    onFound(info);
        UIManager.Instance.UpdateInventoryInfoItem(level, info);
        UIManager.Instance.ShowItemInfoCanvas(info);

        gameObject.SetActive(false);
    }

    public override void OnLookAt()
    {
        isLookingAt = true;
    }
}
