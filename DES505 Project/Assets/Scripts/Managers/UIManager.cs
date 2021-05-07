﻿using System.Collections;
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
    public UIPromptCanvas promptCanvas;
    public UIPromptTextCanvas promptTextCanvas;
    public UIPauseMenuCanvas pauseMenuCanvas;

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
        promptCanvas.gameObject.SetActive(false);
        promptTextCanvas.gameObject.SetActive(false);
        pauseMenuCanvas.gameObject.SetActive(false);

        cameraCanvas.defaultVolume.SetActive(true);
        cameraCanvas.cameraVolume.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonNameInventory))
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING && !cameraCanvas.gameObject.activeInHierarchy)
            {
                inventoryCanvas.ActivateCanvas();
            }
        }
        if(Input.GetButtonDown(GameConstants.k_ButtonNameBack))
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING && !cameraCanvas.gameObject.activeInHierarchy)
            {
                pauseMenuCanvas.ActivateCanvas();
            }
        }
    }

    public void ShowItemInfoCanvas(EvidenceItem item)
    {
        inventoryCanvas.DeactivateCanvas();
        promptCanvas.DeactivateCanvas();
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
            promptCanvas.DeactivateCanvas();
            cameraCanvas.ActivateCanvas();
        }
        else
        {
            cameraCanvas.DeactivateCanvas();
        }
    }

    public void ShowGameoverCanvas()
    {
        promptCanvas.DeactivateCanvas();
        gameoverCanvas.ActivateCanvas();
    }

    public void ShowBillboardCanvas(Sprite image)
    {
        promptCanvas.DeactivateCanvas();
        billboardCanvas.sprite = image;
        billboardCanvas.ActivateCanvas();
    }

    public void ShowPromptCanvas(bool isVisible, string str = "Interact")
    {
        promptCanvas.UpdateText(str);
        if (isVisible)
        {
            promptCanvas.ActivateCanvas();
        }
        else
        {
            promptCanvas.DeactivateCanvas();
        }
    }

    public void ShowPromptTextCanvas(string str = "")
    {
        promptTextCanvas.UpdateText(str);
        promptTextCanvas.ActivateCanvas();
    }
}
