using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

    private void Start()
    {
        cameraBase = GameObject.Find("Base");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (Array.Exists(gameManager.Players, player => player.Character == Character.Ninja))
        {
            Destroy(GameObject.Find("Space Map"));
            GameObject.Find("3 Stage Map").transform.Translate(new Vector3(39, 0, 0));
        }
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
            Destroy(this);
        }
    }
    
    private void FixedUpdate()
    {
        cameraBase.transform.localRotation = Quaternion.Euler(rightStickPosition.y * 30f, -rightStickPosition.x * 30f,0);
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
