using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvidenceLink : MonoBehaviour
{
    public string url;

    public void JumpToLink()
    {
        if(url != null)
            Application.OpenURL(url);
    }
}
