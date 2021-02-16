using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    public static PlayerController playerController;
    [Tooltip("instances of all the photo evidences in the scene")]
    public PhotoPoint[] points;
    [SerializeField]
    int foundNumber = 0;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        for(int i=0; i<points.Length; ++i)
        {
            points[i].onFound += OnFound;
        }
    }

    void OnFound()
    {
        ++foundNumber;
    }
}
