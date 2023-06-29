using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net.Security;
using Unity.Netcode;

public class NetworkManagerUi : MonoBehaviour
{
    [SerializeField] private Button server;
    [SerializeField] private Button host;
    [SerializeField] private Button client;
    private void Awake()
    {
        server.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        client.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
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
