using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorLerp : MonoBehaviour
{
    public Color startCol;
    public Color endCol;

    private Text thisText;

    public int time;
    private float timeSince;
    public float prebake;
    // Start is called before the first frame update
    void Start()
    {
        thisText = GetComponent<Text>();
        timeSince -= prebake;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSince <= time)
        {
            float t = (float) timeSince / time;
            thisText.color = Color.Lerp(startCol, endCol, t);
            timeSince += Time.deltaTime;
        }
    }
}
