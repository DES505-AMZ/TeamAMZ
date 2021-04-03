using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraCanvas : UICanvas
{
    public GameObject defaultVolume;
    public GameObject cameraVolume;

    public override void ActivateCanvas()
    {
        isActive = true;
        gameObject.SetActive(true);
        defaultVolume.SetActive(false);
        cameraVolume.SetActive(true);
    }

    public override void DeactivateCanvas()
    {
        isActive = false;
        gameObject.SetActive(false);
        defaultVolume.SetActive(true);
        cameraVolume.SetActive(false);
    }
}
