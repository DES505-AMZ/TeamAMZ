using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject endMarker;
    private Vector3 startPos;
    private Vector3 endPos;
    public int transitionTime = 5;
    private float currentTime;
    public bool loop;
    private bool returnJourney;
    private bool stopped;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = endMarker.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            float t = currentTime / transitionTime;
            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (currentTime >= transitionTime)
            {
                if (loop)
                {
                    returnJourney = true;
                } else
                {
                    stopped = true;
                }
            }
            
            if (currentTime <= 0)
            {
                returnJourney = false;
            }

            if (!returnJourney)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
    }
}
