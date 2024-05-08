using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : NetworkBehaviour
{
    private int numberOfPlayers = 0;
    public static GameManager Instance { get; private set; }

    public Player player1Prefab;
    public Player player2Prefab;
    public Vector3 spawnPoint;

    public List<Player> playerList;

    public override void OnNetworkSpawn()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += NewPlayerConnected;
    }

    //Whenever new player connects, this function happens
    public void NewPlayerConnected(ulong id)
    {
        numberOfPlayers++;

        if(numberOfPlayers == 1)
        {
            Player player1 = Instantiate(player1Prefab, spawnPoint, Quaternion.identity);
            player1.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
            playerList.Add(player1);
 
        }
        else if (numberOfPlayers == 2)
        {
            Player player2 = Instantiate(player2Prefab, spawnPoint, Quaternion.identity);
            player2.GetComponent<NetworkObject>().SpawnAsPlayerObject(id);
            playerList.Add(player2);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

