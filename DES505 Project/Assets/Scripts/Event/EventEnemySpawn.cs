using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnemySpawn : TriggerEvent
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void OnEvent()
    {
        gameObject.SetActive(true);
    }
}
