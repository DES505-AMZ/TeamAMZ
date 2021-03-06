﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : Interactable
{
    [Tooltip("Does player need to activate a trigger(key, switch or card) to open the door?")]
    public bool hasTrigger = false;
    [Tooltip("Can player close the door by interacting?")]
    public bool canClose = false;
    
    protected bool m_isOpen;
    protected Ray m_playerViewRay;
    public Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnInteraction(Ray ray)
    {
        base.OnInteraction(ray);

        m_playerViewRay = ray;

        if(!m_isOpen && CanOpenByPlayer())
        {
            DoorOpen();
        }
        else if(canClose && m_isOpen)
        {
            DoorClose();
        }
    }

    protected virtual bool CanOpenByPlayer()
    {
        return true;
    }

    public virtual void DoorOpen()
    {
        if(animator)
            animator.SetTrigger("OpenDoor");
        m_isOpen = true;
    }

    public virtual void DoorClose()
    {
        if(animator)
            animator.SetTrigger("CloseDoor");
        m_isOpen = false;


    }

}
