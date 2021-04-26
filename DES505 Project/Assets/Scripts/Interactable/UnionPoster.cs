using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionPoster : Interactable
{
    public Sprite image;

    public override void OnInteraction(Ray ray)
    {
        base.OnInteraction(ray);

        UIManager.Instance.ShowBillboardCanvas(image);
        TannoySystem.Instance.PlayPosterFound();
    }

    public override void OnLookAt()
    {
        UIManager.Instance.ShowPromptCanvas(true);
    }

    public override void OnLookExit()
    {
        UIManager.Instance.ShowPromptCanvas(false);
    }
}
