using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TannoyTrigger : MonoBehaviour
{
    public AudioClip audioClip;
    BoxCollider collider;
    bool triggered = false;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
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
        if (!triggered)
        {
            TannoySystem.Instance.PlayTannoyAudio(audioClip);
            triggered = true;
        }
    }
}
