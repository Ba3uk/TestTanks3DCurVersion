using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuffManager : NetworkBehaviour {
    public GameObject buffBox;
    private float timeLastSpawnBox =-17f,timeToNewBox = 10f;
    public Vector3[] spawnsPoint;

    void Start () {
        spawnsPoint = new Vector3[transform.childCount - 1];
        for (int i = 0; i< transform.childCount - 1; i++)
        {
            spawnsPoint[i] = transform.GetChild(i).position;
        }

    }
	
	void Update () {
        if(timeLastSpawnBox <= Time.fixedTime - timeToNewBox)
        {
            timeLastSpawnBox = Time.fixedTime;
            int rand = Random.Range(0, spawnsPoint.Length);
            if (isServer)
                RpcSpawnBuffBox(rand);
            else
                CmdSpawnBuffBox(rand); 
        }
    }

    [Command]
    public void CmdSpawnBuffBox(int i)
    {
        
      RpcSpawnBuffBox(i);
    }

    [ClientRpc]
    public void RpcSpawnBuffBox(int i)
    {

        GameObject lootBox = Instantiate(buffBox, spawnsPoint[i] + Vector3.up, new Quaternion()) as GameObject;
        Destroy(lootBox, 15f);

    }
}
