using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class MenuControl : MonoBehaviour
{
    [SerializeField]
    private GameObject platformPrefab;
    [SerializeField]
    private GameObject platform;
    public int playerNumber;
    public Character CharacterSelected;
    
    private GameObject cameraBase;
    private MenuManager menuManager;
    private GameManager gameManager;
    private SpriteRenderer platformSpriteRenderer;
    private MeshFilter platformMeshFilter;
    private MeshRenderer platformMeshRender;
    private PlayerInput playerInput;

    private Vector2 rightStickPosition;
    private Vector2 leftStickPosition;
    
    private void Awake()
    {
        cameraBase = GameObject.Find("Base");
        menuManager = GameObject.Find("PlayerInputManager").GetComponent<MenuManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerInput = GetComponent<PlayerInput>();
        playerNumber = menuManager.nbOfPlayers;
        CharacterSelected = Character.None;
            
        if (playerNumber == 0)
            platform = Instantiate(platformPrefab, new Vector3(-35, -30), Quaternion.identity);
        else
            platform = Instantiate(platformPrefab, new Vector3(35, -30), Quaternion.identity);
        
        platformSpriteRenderer = platform.GetComponentInChildren<SpriteRenderer>();
        platformMeshFilter = platform.GetComponentInChildren<MeshFilter>();
        platformMeshRender = platform.GetComponentInChildren<MeshRenderer>();
    }

    private void OnLeftStick(InputValue value)
    {
        if (!gameManager.Players[playerNumber].Ready)
            leftStickPosition = value.Get<Vector2>();
    }

    public void OnRightStick(InputValue value)
    {
        rightStickPosition = value.Get<Vector2>();
    }
    
    private void OnA(InputValue value)
    {
        if (value.isPressed)
        {
            if (CharacterSelected != Character.None)
            {
                gameManager.Players[playerNumber].Ready = true;
                gameManager.Players[playerNumber].Character = CharacterSelected;
                gameManager.Players[playerNumber].PlayerInput = playerInput;
            }
        }
    }

    private void OnB(InputValue value)
    {
        if (value.isPressed)
            gameManager.Players[playerNumber].Ready = false;
    }
    
    private void FixedUpdate()
    {
        cameraBase.transform.localRotation = Quaternion.Euler(rightStickPosition.y * 30f, -rightStickPosition.x * 30f,0);
        if (!gameManager.Players[playerNumber].Ready) 
            transform.Translate(new Vector3(leftStickPosition.x,leftStickPosition.y));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 15)
            return;

        CharacterSelect characterSelect = other.gameObject.GetComponent<CharacterSelect>();
        CharacterSelected = characterSelect.Character;

        bool characterIsSprite = characterSelect.CharacterRenderType == CharacterRenderType.Sprite;
        if (characterIsSprite)
            platformSpriteRenderer.sprite = characterSelect.Sprite;
        else
        {
            Transform platformMeshTransform = platformMeshFilter.gameObject.transform;
            platformMeshTransform.localPosition = characterSelect.Position;
            platformMeshTransform.eulerAngles = characterSelect.Rotation;
            platformMeshTransform.localScale = characterSelect.Scale;
            platformMeshFilter.mesh = characterSelect.Mesh;
            platformMeshRender.material = characterSelect.Material;
        }
        
        platformSpriteRenderer.enabled = characterIsSprite;
        platformMeshRender.enabled = !characterIsSprite;
    }

    

    private void OnTriggerExit2D(Collider2D other)
    {
        platformSpriteRenderer.enabled = false;
        platformMeshRender.enabled = false;
        CharacterSelected = Character.None;
    }
}
