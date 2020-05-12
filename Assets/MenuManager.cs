using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject Prefab1;

    public GameObject Prefab2;

    public int nbOfPlayers = 0;

    void OnPlayerJoined()
    {
        nbOfPlayers++;
    }

    void Start()
    {
        PlayerInput.Instantiate(Prefab1,0);
    }
}
