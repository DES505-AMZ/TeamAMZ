using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate void CollectableAction(EvidenceItem item);
public class CollectableItem : Interactable
{
    public bool isFound { get; private set; }

    //public event CollectableAction onFound;

    public GameConstants.LevelArea level;
    public EvidenceItem info;
    public string promptText = "Collect Evidence";

    private void Start()
    {

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
        UIManager.Instance.ShowPromptCanvas(true, promptText);
    }

    public override void OnLookExit()
    {
        UIManager.Instance.ShowPromptCanvas(false);
    }
}
