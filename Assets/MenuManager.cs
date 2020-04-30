using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject platformPrefab;
    
    public int nbOfPlayers = 0;

    void OnPlayerJoined()
    {
        nbOfPlayers++;
    }
}
