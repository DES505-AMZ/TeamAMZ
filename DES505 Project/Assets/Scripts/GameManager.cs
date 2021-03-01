using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static PlayerInputHandler inputHandler;
    static PlayerController playerController;

    //public static GameManager gameManager
    //{
    //    get
    //    {
    //        if (instance == null)
    //            instance = new GameManager();
    //        return instance;
    //    }
    //}
    //static GameManager instance;

    void Start()
    {
        inputHandler = FindObjectOfType<PlayerInputHandler>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0f;
        inputHandler.canProcessInput = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
        inputHandler.canProcessInput = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
