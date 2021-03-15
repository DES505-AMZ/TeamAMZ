using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToKill : MonoBehaviour
{
    public float time;
    public float freezeLength = 1;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        StartCoroutine(destroyAfterTime());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb)
        {
            if (rb.velocity.sqrMagnitude == 0)
            {
                StartCoroutine(checkStillMotionLess());
            }
        }
    }

    IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(time);

        GameObject.Destroy(this.gameObject);
    }

    IEnumerator checkStillMotionLess()
    {
        yield return new WaitForSeconds(freezeLength);
        if (rb.velocity.sqrMagnitude == 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
