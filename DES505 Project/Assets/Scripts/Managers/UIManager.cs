using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public UIInventoryCanvas inventoryCanvas;
    public UIItemInfoCanvas itemInfoCanvas;
    public Canvas cameraCanvas;

    GameObject currentCanvas;

    protected override void Awake()
    {
        base.Awake();
        inventoryCanvas.gameObject.SetActive(true);
    }

    void Start()
    {
        inventoryCanvas.gameObject.SetActive(false);
        itemInfoCanvas.gameObject.SetActive(false);
        cameraCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonNameInventory))
        {
            if (inventoryCanvas.gameObject.activeInHierarchy)
            {
                inventoryCanvas.gameObject.SetActive(false);
                GameManager.Instance.ResumeGame();
            }
            else
            {
                if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
                {
                    GameManager.Instance.PauseGame();
                    inventoryCanvas.gameObject.SetActive(true);
                    currentCanvas = inventoryCanvas.gameObject;
                }
            }
        }

    }

    public void ShowItemInfoCanvas(EvidenceItem item)
    {
        if (currentCanvas != null)
            currentCanvas.SetActive(false);
        itemInfoCanvas.ShowItemInfo(item);
        currentCanvas = itemInfoCanvas.gameObject;
    }

    public void UpdateInventoryInfoPhoto(GameConstants.LevelArea level)
    {
        inventoryCanvas.UpdatePhotoInfo(level);
    }

    public void UpdateInventoryInfoItem(GameConstants.LevelArea level, EvidenceItem item)
    {
        inventoryCanvas.UpdateItemInfo(level, item);
    }

    public void SetCameraCanvasVisible(bool isVisible)
    {
        if(isVisible)
            cameraCanvas.gameObject.SetActive(true);
        else
            cameraCanvas.gameObject.SetActive(false);
    }

}
