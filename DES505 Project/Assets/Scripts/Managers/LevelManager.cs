using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Player player;
    public TannoySystem tannoy;
    public PhotoPoint[] photoPoints;
    int numPhotoFound = 0;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        tannoy = FindObjectOfType<TannoySystem>();
        photoPoints = FindObjectsOfType<PhotoPoint>();

        player.onCaught += OnPlayerCaught;

        for (int i=0; i<photoPoints.Length; ++i)
        {
            photoPoints[i].onFound += OnPhotoFound;
            player.onPhoto += photoPoints[i].OnPhotoTake;
        }
    }

    private void Start()
    {
        UIManager.Instance.onButtonBackToCheckPoint += OnBackToCheckPoint;
    }

    void OnPhotoFound(PhotoPoint point)
    {
        player.onPhoto -= point.OnPhotoTake;
        UIManager.Instance.UpdateInventoryInfoPhoto(point.level);
        tannoy.PlayPhotoFound(numPhotoFound++);
    }

    void OnPlayerCaught()
    {
        UIManager.Instance.ShowGameoverCanvas();
    }

    void OnBackToCheckPoint()
    {
        player.Initialize(CheckPointManager.Instance.CurrentCheckPoint.playerRespawnLocation);
        CheckPointManager.Instance.ResetCurrentCheckPoint();
    }
}
