using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBack : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown(GameConstants.k_ButtonNameAim))
            gameObject.SetActive(false);
    }
}
