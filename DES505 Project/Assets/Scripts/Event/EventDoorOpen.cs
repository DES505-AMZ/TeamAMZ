using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDoorOpen : TriggerEvent
{
    float openTimeTotal = 0.8f;
    DoorBase door;   
    float openTimeCount = 0f;
    bool isOpening = false;

    private void Start()
    {
        door = GetComponent<DoorBase>();
    }

    private void Update()
    {
        if(isOpening)
        {
            openTimeCount += Time.deltaTime;
            if (openTimeCount >= openTimeTotal)
            {
                isOpening = false;
                door.DoorClose();
            }
        }
    }

    public override void OnEvent()
    {
        door.DoorOpen();
        isOpening = true;
        openTimeCount = 0f;
    }
}
