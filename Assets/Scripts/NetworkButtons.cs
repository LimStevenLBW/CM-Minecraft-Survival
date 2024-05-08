using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    public GameObject cameraObj;
    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        cameraObj.SetActive(false);
        gameObject.SetActive(false);

    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        cameraObj.SetActive(false);
        gameObject.SetActive(false);
    }
}
