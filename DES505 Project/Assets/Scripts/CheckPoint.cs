using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    public Transform playerRespawnLocation;

    public UnityAction<CheckPoint> onTrigger;
    public UnityAction onRestart;

    bool triggered;

    void Start()
    {
        triggered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!triggered)
        {
            if (other.gameObject == PlayerController.Instance.gameObject)
                Trigger();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(triggered)
        {
            triggered = false;
        }
    }

    void Trigger()
    {
        triggered = true;

        if (onTrigger != null)
            onTrigger(this);

        PlayerController.Instance.onCaught += OnResetCheckPoint;
    }

    void OnResetCheckPoint()
    {
        if (CheckPointManager.Instance.CurrentCheckPoint == this)
        {
            if (onRestart != null)
                onRestart.Invoke();

            PlayerController.Instance.Restart(playerRespawnLocation);
        }
    }
}
