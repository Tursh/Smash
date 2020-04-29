using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MenuControl : MonoBehaviour
{
    private Vector2 RightStickPosition;
    private Vector2 LeftStickPosition;
    private PlayerControls PlayerControls;
    private GameObject cameraBase;

    private void Awake()
    {
        PlayerControls = new PlayerControls();
        cameraBase = GameObject.Find("Base");
        PlayerControls.Menu.RightStick.performed += RightStickOnperformed;
        PlayerControls.Menu.LeftStick.performed += LeftStickOnperformed;
    }

    private void LeftStickOnperformed(InputAction.CallbackContext obj)
    {
        LeftStickPosition = obj.ReadValue<Vector2>();
    }

    public void RightStickOnperformed(InputAction.CallbackContext obj)
    {
        RightStickPosition = obj.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        cameraBase.transform.localRotation = Quaternion.Euler(RightStickPosition.y * 30f, -RightStickPosition.x * 30f,0);
        transform.Translate(new Vector3(LeftStickPosition.x,LeftStickPosition.y));
    }
    
    
    
    private void OnEnable()
    {
        PlayerControls.Enable();
    }
}
