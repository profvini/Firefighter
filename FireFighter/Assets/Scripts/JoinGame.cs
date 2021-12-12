using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class JoinGame : MonoBehaviour
{
    public NetworkManager networkManager;
    public GameObject waitingHost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startHost()
    {
        networkManager.StartHost();
    }

    public void startClient()
    {
        waitingHost.SetActive(true);
        networkManager.StartClient();
    }
}
