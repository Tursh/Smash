using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;

public class MenuControl : MonoBehaviour
{
    private Vector2 RightStickPosition;
    private Vector2 LeftStickPosition;
    private PlayerControls PlayerControls;
    private GameObject cameraBase;
    private MenuManager MenuManager;
    public GameObject PlatformPrefab;
    public GameObject Platform;
    public int playerNumber;
    private SpriteRenderer platformSpriteRenderer;
    private MeshFilter platformMeshFilter;
    private MeshRenderer platformMeshRender;
    private PlayerInput PlayerInput;
    private InputUser inputUser;

    private void Awake()
    {
        inputUser = default;
        PlayerControls = new PlayerControls();
        cameraBase = GameObject.Find("Base");
        MenuManager = GameObject.Find("GameManager").GetComponent<MenuManager>();
        PlayerInput = GetComponent<PlayerInput>();
        
        InputUser.PerformPairingWithDevice(Gamepad.all[playerNumber], inputUser);
        PlayerControls.Menu.RightStick.performed += RightStickOnperformed;
        PlayerControls.Menu.LeftStick.performed += LeftStickOnperformed;
            
        if (playerNumber == 1)
            Platform = Instantiate(PlatformPrefab, new Vector3(-35, -30), Quaternion.identity);
        else
            Platform = Instantiate(PlatformPrefab, new Vector3(35, -30), Quaternion.identity);
        
        platformSpriteRenderer = Platform.GetComponentInChildren<SpriteRenderer>();
        platformMeshFilter = Platform.GetComponentInChildren<MeshFilter>();
        platformMeshRender = Platform.GetComponentInChildren<MeshRenderer>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 15)
            return;

        CharacterSelect characterSelect = other.gameObject.GetComponent<CharacterSelect>();
        
        if (characterSelect.IsSprite)
        {
            platformSpriteRenderer.sprite = characterSelect.Sprite;
        }
        else
        {
            platformMeshFilter.mesh = characterSelect.mesh;
            platformMeshRender.material = characterSelect.Material;
        }
        
        platformSpriteRenderer.enabled = characterSelect.IsSprite;
        platformMeshRender.enabled = !characterSelect.IsSprite;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        platformSpriteRenderer.enabled = false;
        platformMeshRender.enabled = false;
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
    }
}
