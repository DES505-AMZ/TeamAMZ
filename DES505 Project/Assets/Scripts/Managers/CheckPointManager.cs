using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointManager : Singleton<CheckPointManager>
{
    public CheckPoint CurrentCheckPoint { get { return checkPoints.Count > 0 ? checkPoints[curIndex] : null; } }

    public List<CheckPoint> checkPoints = new List<CheckPoint>();
    public int curIndex = 0;

    public UnityAction onCheckPointReset;

    public bool resetAllCheckPoints = false;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < transform.childCount; ++i)
        {
            CheckPoint checkpoint = transform.GetChild(i).GetComponent<CheckPoint>();
            if (checkpoint != null)
            {
                checkpoint.onTrigger += OnCheckPointTriggered;
                checkPoints.Add(checkpoint);
            }
        }
    }

    public void OnCheckPointTriggered(CheckPoint newCheckPoint)
    {
        curIndex = checkPoints.IndexOf(newCheckPoint);
    }

    public void ResetCurrentCheckPoint()
    {
        if(!resetAllCheckPoints)
            CurrentCheckPoint.OnResetCheckPoint();
        else
        {
            foreach(CheckPoint point in checkPoints)
            {
                point.OnResetCheckPoint();
            }
        }

        if(onCheckPointReset != null)
        {
            onCheckPointReset();
        }
    }
}
