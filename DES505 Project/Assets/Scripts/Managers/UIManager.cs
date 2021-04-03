using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    public UIInventoryCanvas inventoryCanvas;
    public UIItemInfoCanvas itemInfoCanvas;
    public UICameraCanvas cameraCanvas;
    public UIGameoverCanvas gameoverCanvas;
    public UIBillboardCanvas billboardCanvas;

    public UnityAction onButtonBackToCheckPoint;

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
        gameoverCanvas.gameObject.SetActive(false);
        billboardCanvas.gameObject.SetActive(false);

        cameraCanvas.defaultVolume.SetActive(true);
        cameraCanvas.cameraVolume.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonNameInventory))
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
            {
                inventoryCanvas.ActivateCanvas();
            }
        }
    }

    public void ShowItemInfoCanvas(EvidenceItem item)
    {
        itemInfoCanvas.item = item;
        itemInfoCanvas.ActivateCanvas();
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
        if (isVisible)
        {
            cameraCanvas.ActivateCanvas();
        }
        else
        {
            cameraCanvas.DeactivateCanvas();
        }
    }

    public void ShowGameoverCanvas()
    {
        gameoverCanvas.ActivateCanvas();
    }

    public void ShowBillboardCanvas(Sprite image)
    {
        billboardCanvas.sprite = image;
        billboardCanvas.ActivateCanvas();
    }
}
