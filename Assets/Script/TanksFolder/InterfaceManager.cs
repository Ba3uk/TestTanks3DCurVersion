using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InterfaceManager : NetworkBehaviour {

    private const string PREFIX_SCORE = "Score: ";
    [SyncVar]
    private int Score = 0;
    private Text text;
    [SyncVar]
    private bool isPause = false;
    private bool currState = false;
    void Start()
    {
        text = GameObject.Find("ScoreText").GetComponent<Text>();
        text.text = PREFIX_SCORE + "0";
    }

    public void Update()
    {
        if(currState != isPause)
        {
            CmdClickPause();
        }
    }

    public void PrintScore()
    {
        CmdSendPrintScore();
    }


    [Command]
    void CmdSendPrintScore()
    {
        RpcSendPrintScore();
    }

    [ClientRpc]
   void RpcSendPrintScore()
    {
        text.text = PREFIX_SCORE + Score.ToString();
    }



    public void AddScore()
    {
        Score++;
        PrintScore();
    }
    [Client]
    public void ClickPause()
    {
        CmdClickPause();

        }

    [Command]
    public void CmdClickPause()
    {
        RpcOnClickPause();
        }

    [ClientRpc]
    public void RpcOnClickPause()
    {
        if (isPause)
        {

            isPause = false;
           
            Time.timeScale = 1;
        }
        else
        {
            
            isPause = true;
            Time.timeScale = 0;
        }
        currState = isPause;
    }






}
