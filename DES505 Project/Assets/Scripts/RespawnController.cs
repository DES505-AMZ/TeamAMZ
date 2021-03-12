using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnController : MonoBehaviour
{
    public CheckPoint respawningCheckPoint = null;

    public UnityAction onRespawn;

    Vector3 initialPosition;
    Quaternion initialRotation;

    void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (respawningCheckPoint == null)
            Debug.LogWarning(gameObject.ToString() + " is not assigned to any checkpoint");
        else
            respawningCheckPoint.onRestart += OnRespawn;
    }
    public void OnRespawn()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        if(onRespawn != null)
            onRespawn();
    }
}