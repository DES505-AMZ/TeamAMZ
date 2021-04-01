using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate void CollectableAction(EvidenceItem item);
public class CollectableItem : Interactable
{
    //MeshRenderer meshRenderer;
    bool isLookingAt = false;
    public bool isFound { get; private set; }

    //public event CollectableAction onFound;

    public GameConstants.LevelArea level;
    public EvidenceItem info;

    private void Start()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        //if(isLookingAt)
        //    meshRenderer.material.SetColor("_BaseColor", Color.green);
        //else
        //    meshRenderer.material.SetColor("_BaseColor", Color.red);
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
