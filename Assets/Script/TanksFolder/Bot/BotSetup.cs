using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BotSetup : NetworkBehaviour {

    public Behaviour[] componentsToDisable;

    private const string BOT_PRIFIX_ID = "Bot ";
    private NetworkIdentity id;

    private void Start()
    {
        id = GetComponent<NetworkIdentity>();
        RenameBot(id.netId.ToString());
       // DisableComponents();
    }


    void DisableComponents()
    {
        if (!isServer)
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
    }

    private void RenameBot(NetworkInstanceId netId)
    {
        throw new NotImplementedException();
    }

    public void RenameBot(string _netId)
    {
        transform.name = BOT_PRIFIX_ID + _netId;
    }
}
