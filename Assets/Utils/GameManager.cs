using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Map
{
    Menu = 0,
    Platform3 = 1, 
    Battle = 2,
    Space = 3,
    Spline = 4
}

public class Player
{

    public PlayerInput PlayerInput;
    public Character Character;

    private bool ready;
    public bool Ready
    {
        get => ready; 
        set
        {
            if (ready = value)
                OnReady?.Invoke(this, EventArgs.Empty);
        }
    }
    public int Index => PlayerInput.playerIndex;
    public EventHandler OnReady;
    public Player()
    {
        Ready = false;
    }
}

public class GameManager : MonoBehaviour
{
    public Player[] Players;
    public GameObject[] CharacterPrefabs;
    
    private void Awake()
    {
        Players = new Player[2];
        for (int i = 0; i < Players.Length; ++i)
        {
            Players[i] = new Player();
            Players[i].OnReady += OnPlayerReady;
        }
        DontDestroyOnLoad(this);
    }

    private void OnPlayerReady(object sender, EventArgs eventArgs)
    {
        if (Players.All(player => player.Ready))
            SceneManager.LoadScene(5);
    }

    public void PlayerLoses(uint index)
    {
        SceneManager.LoadScene(6);

        GameObject.Find("Text").GetComponent<Text>().text = "Player " + ((index + 1) % 2 + 1) + " wins";
        Destroy(gameObject);
    }
}
