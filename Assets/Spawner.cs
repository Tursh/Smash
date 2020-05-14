using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int playerIndex;
    public GameObject[] Characters;

    void Awake()
    {
	    GameObject characterSpawned = Instantiate(Characters[
		    (int) GameObject.Find("GameManager").GetComponent<GameManager>().Players[playerIndex].Character] );
	    if (playerIndex == 0)
		    GameObject.Find("P1").GetComponent<GeneralUi>().playerInfo = characterSpawned.GetComponent<PlayerInfo>();
	    else if (playerIndex == 1)
		    GameObject.Find("P2").GetComponent<GeneralUi>().playerInfo = characterSpawned.GetComponent<PlayerInfo>();
	    Destroy(this);
    }
}
