using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class CustomNetworkManager : NetworkManager {

    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        SetIpAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIpAddress()
    {
        string ipAddress = GameObject.Find("InputIP").transform.Find("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            StartCoroutine(SetupMenuSceneButtons());
        }
    }

    public void OpenConnectMenu()
    {
        GameObject.Find("MainMenu").SetActive(false);
        GameObject.Find("JoinGame").SetActive(true);
    }

    private IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("StartGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("StartGame").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("Join").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Join").GetComponent<Button>().onClick.AddListener(OpenConnectMenu);

    }
}
