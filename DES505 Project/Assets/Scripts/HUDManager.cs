using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static PhotoManager photoManager;
    public Text photoText;

    private void Awake()
    {
        photoManager = FindObjectOfType<PhotoManager>();
        photoText = GetComponentInChildren<Text>();
        SetPhotoText(0);
    }

    void Start()
    {
        photoManager.onPhotoFound += OnPhotoFound;
    }

    void OnPhotoFound(int numFound)
    {
        SetPhotoText(numFound);
    }

    void SetPhotoText(int num)
    {
        if (photoText && photoManager)
        {
            photoText.text = "Photos Found: " + num + " / " + photoManager.points.Length;
        }
    }
}
