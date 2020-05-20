using UnityEngine;

public class MenuManager : MonoBehaviour
{ 
    public int nbOfPlayers { get; private set; }
    private GameManager gameManager;

    private void Start()
    {
        nbOfPlayers = -1;
    }

    void OnPlayerJoined()
    {
        nbOfPlayers++;
    }
}
