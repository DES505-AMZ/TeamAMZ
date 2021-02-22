using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PhotoFoundAction(int numFound);
public class PhotoManager : MonoBehaviour
{
    public static PlayerController playerController;
    [Tooltip("instances of all the photo evidences in the scene")]
    public PhotoPoint[] points;

    int foundNumber = 0;

    public event PhotoFoundAction onPhotoFound;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if(points.Length == 0)
        {
            points = FindObjectsOfType<PhotoPoint>();
        }
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
        if(onPhotoFound != null)
            onPhotoFound(foundNumber);
    }
}
