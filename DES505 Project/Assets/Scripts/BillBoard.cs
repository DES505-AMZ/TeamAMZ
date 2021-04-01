using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : Interactable
{
    public override void OnInteraction(Ray ray)
    {
        base.OnInteraction(ray);

        UIManager.Instance.ShowBillboardCanvas();
    }
}
