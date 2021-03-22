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
            if (other.gameObject.tag == GameConstants.k_TagNamePlayer)
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
    }

    public void OnResetCheckPoint()
    {
        //LevelManager.Instance.player.Initialize(playerRespawnLocation);
        if (onRestart != null)
            onRestart.Invoke();
    }
}
