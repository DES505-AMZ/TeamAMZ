using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenByCards : MonoBehaviour
{
    public DoorNormal[] gates;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gates.Length; ++i)
        {
            gates[i].onCheckAllGatesUnlockedAndOpen = OnCheckAllGatesUnlockedAndOpen;
        }
    }

    void OnCheckAllGatesUnlockedAndOpen()
    {
        if (gates.Length <= 0) 
            return;

        bool flag = true;
        for(int i=0; i<gates.Length; ++i)
        {
            flag &= gates[i].isUnlocked;
        }

        if (flag)
            OpenAllGates();
    }

    void OpenAllGates()
    {
        for (int i = 0; i < gates.Length; ++i)
        {
            gates[i].DoorOpen();
        }
    }
}
