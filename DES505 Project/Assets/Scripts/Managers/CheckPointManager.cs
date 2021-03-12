using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : Singleton<CheckPointManager>
{
    public List<CheckPoint> CheckPoints { get { return checkPoints; } }
    public CheckPoint CurrentCheckPoint { get { return checkPoints.Count > 0 ? checkPoints[curIndex] : null; } }

    List<CheckPoint> checkPoints = new List<CheckPoint>();
    public int curIndex = 0;

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
}
