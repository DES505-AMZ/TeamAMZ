using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void OnInteraction()
    {
        Debug.Log("Interact with " + gameObject.name);
    }

    public virtual void OnLookAt()
    {

    }
}
