using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuffBox : NetworkBehaviour {

    public List<Buff> buffList;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            int num = Random.Range(1, 100);
            if( 0 < num && num < 30)
            {
                InstBuff(0,other.transform);
            }
            else if (31 < num && num < 75)
            {
                InstBuff(1, other.transform);
            }
            else if (76 < num && num < 100)
            {
                InstBuff(2, other.transform);
            }

            
        }
    }

    private void InstBuff( int num ,Transform t )
    {
        Buff buff = Instantiate(buffList[num], t).transform.GetComponent<Buff>();
        if(t.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            buff.SetImage();
        Destroy(gameObject);
    }
}
