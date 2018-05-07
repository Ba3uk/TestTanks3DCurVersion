using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup :NetworkBehaviour  {

    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    MeshRenderer[] meshRender;
    [SerializeField]
    Material material;
    private static int coutn = 0;

    void Start()
    {
        DisableComponents();

    }

    void DisableComponents()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
        else
        {
            foreach (MeshRenderer mr in meshRender)
            {
                
                mr.material = material;
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkBehaviour>().netId.ToString();
        TankController player = GetComponent<TankController>();
        GameManager.RegisterPlayer(_netID, player);
    }

    private void OnDisable()
    {

        GameManager.UnRegisterPlayer(transform.name);
        if(GameManager.players.Count == 0)
        {
            NetworkManager.singleton.StopHost();

        }
    }




}
