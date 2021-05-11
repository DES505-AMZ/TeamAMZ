using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TannoyTrigger : MonoBehaviour
{
    public AudioClip audioClip;
    public bool resetTrigger = false;
    Collider collider;
    bool triggered = false;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void Start()
    {
        CheckPointManager.Instance.onCheckPointReset += OnResetTrigger;
    }

    void OnResetTrigger()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && (!triggered || resetTrigger))
        {
            TannoySystem.Instance.PlayTannoyAudio(audioClip);
            triggered = true;
        }
    }
}
