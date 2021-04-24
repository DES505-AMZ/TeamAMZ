using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPromptTextCanvas : UICanvas
{
    public Text text;
    public float time = 1f;
    float timeElapsed = 0f;

    private void Start()
    {
        timeElapsed = 0f;
    }

    private void Update()
    {
        if (isActive)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= time)
                DeactivateCanvas();
        }
    }

    public void UpdateText(string str)
    {
        text.text = str;
    }

    public override void ActivateCanvas()
    {
        if (isActive)
            return;
        isActive = true;
        gameObject.SetActive(true);
    }

    public override void DeactivateCanvas()
    {
        isActive = false;
        timeElapsed = 0f;
        gameObject.SetActive(false);        
    }
}
