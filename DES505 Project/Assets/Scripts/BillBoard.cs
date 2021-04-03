using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : Interactable
{
    public Sprite image;

    public override void OnInteraction(Ray ray)
    {
        base.OnInteraction(ray);

        UIManager.Instance.ShowBillboardCanvas(image);
    }
}
