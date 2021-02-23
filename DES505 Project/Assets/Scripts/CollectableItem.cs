﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void CollectableAction(CollectableItem item);
public class CollectableItem : Interactable
{
    MeshRenderer renderer;
    bool isLookingAt = false;
    public bool isFound { get; private set; }

    public event CollectableAction onFound;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        if(isLookingAt)
            renderer.material.SetColor("_BaseColor", Color.green);
        else
            renderer.material.SetColor("_BaseColor", Color.red);
        isLookingAt = false;
    }

    public override void OnInteraction()
    {
        base.OnInteraction();

        isFound = true;
        if (onFound != null)
            onFound(this);

        gameObject.SetActive(false);
    }

    public override void OnLookAt()
    {
        isLookingAt = true;
    }
}