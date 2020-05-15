using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapControl : MonoBehaviour
{
    private GameObject cameraBase;
    private GameManager gameManager;

    private Vector2 rightStickPosition;
    private Vector2 leftStickPosition;

    private Map SelectedMap;
    
    private void Awake()
    {
        cameraBase = GameObject.Find("Base");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnLeftStick(InputValue value)
    {
        leftStickPosition = value.Get<Vector2>();
    }

    public void OnRightStick(InputValue value)
    {
        rightStickPosition = value.Get<Vector2>();
    }
    
    private void OnA(InputValue value)
    {
        if (value.isPressed && SelectedMap != Map.Menu)
        {
            SceneManager.LoadScene((int) SelectedMap);
        }
    }
    
    private void FixedUpdate()
    {
        transform.Translate(new Vector3(leftStickPosition.x,leftStickPosition.y));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SelectedMap = other.gameObject.GetComponent<MapSelect>().CorrespondingMap;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        SelectedMap = Map.Menu;
    }
}
