using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float lookSensitivity = 2f;
    public bool canProcessInput = true;

    void Start()
    {
        GameManager.Instance.OnGameStateChange += HandleGameStatePaused;
    }

    public Vector3 GetMoveInput()
    {
        if (canProcessInput)
        {
            Vector3 move = new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal), 0f, Input.GetAxisRaw(GameConstants.k_AxisNameVertical));

            move = Vector3.ClampMagnitude(move, 1f);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return GetLookAxis(GameConstants.k_MouseAxisNameHorizontal);
    }

    public float GetLookInputsVertical()
    {
        return GetLookAxis(GameConstants.k_MouseAxisNameVertical);
    }

    public bool GetCrouchInputDown()
    {
        if (canProcessInput)
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameCrouch);
        }

        return false;
    }

    public bool GetCrouchInputReleased()
    {
        if (canProcessInput)
        {
            return Input.GetButtonUp(GameConstants.k_ButtonNameCrouch);
        }

        return false;
    }

    public bool GetInteractInputDown()
    {
        if (canProcessInput)
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameInteract);
        }

        return false;
    }

    public bool GetInteractInputReleased()
    {
        if (canProcessInput)
        {
            return Input.GetButtonUp(GameConstants.k_ButtonNameInteract);
        }

        return false;
    }

    public bool GetAimInputDown()
    {
        if (canProcessInput)
        {
            return Input.GetButtonDown(GameConstants.k_ButtonNameAim);
        }

        return false;
    }

    public bool GetAimInputReleased()
    {
        if (canProcessInput)
        {
            return Input.GetButtonUp(GameConstants.k_ButtonNameAim);
        }

        return false;
    }

    float GetLookAxis(string mouseInputName)
    {
        if(canProcessInput)
        {
            float i = Input.GetAxisRaw(mouseInputName);

            i *= lookSensitivity;
            i *= 0.01f;
            return i;
        }
        return 0;
    }

    void HandleGameStatePaused(GameManager.GameState currentState, GameManager.GameState preState)
    {
        if(currentState == GameManager.GameState.PAUSED && preState == GameManager.GameState.RUNNING)
        {
            canProcessInput = false;
        }
        else if (currentState == GameManager.GameState.RUNNING && preState == GameManager.GameState.PAUSED)
        {
            canProcessInput = true;
        }
    }

}
