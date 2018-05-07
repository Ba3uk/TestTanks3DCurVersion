using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientPlayerControl : TankController {

    private TankShoot ts;
    private Vector2 CurTankDir = Vector2.zero;

    private void Start()
    {
        ts = GetComponent<TankShoot>();
        InitializationTank();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            MoveControll();
            SoundConroller();
            if (Input.GetKeyDown(KeyCode.Space))
                ts.ClientFire();
        }
        else
        {
            if (CurTankDir.x != 0 || CurTankDir.y != 0)
            {

                Move();
            }
            else
            {
                rb.velocity = Vector3.zero;
            } 
        }

    }



    void MoveControll()
    {

        float ver = Input.GetAxisRaw("Vertical");
        float hor = Input.GetAxisRaw("Horizontal");

        if (ver != 0 && hor != 0) return;

        if (ver + hor == 0)
        {
            rb.velocity = Vector3.zero;
        }

        if (CurTankDir.x != hor || CurTankDir.y != ver)
        {
            CurTankDir = new Vector2(hor, ver);
            CmdSendCurDir(CurTankDir);
        }

        if (hor != 0)
        {
            if (hor < 0)
            {
                CurrentTurn = TurnPosition.Right;
                CmdTurnPos(CurrentTurn);
            }
            else
            {
                CurrentTurn = TurnPosition.Left;
                CmdTurnPos(CurrentTurn);
            }
            Move();

        }
        if (ver != 0)
        {
            if (ver < 0)
            {
                CurrentTurn = TurnPosition.Down;
                CmdTurnPos(CurrentTurn);
            }
            else
            {
                CurrentTurn = TurnPosition.Up;
                CmdTurnPos(CurrentTurn);
            }
            
            Move();
        }
    }

    [Command]
    private void CmdTurnPos(TurnPosition turn)
    {
        RpcTurnTank(turn);
    }

    [ClientRpc]
    private void RpcTurnTank(TurnPosition turn)
    {
        CurrentTurn = turn;
    }


    [Command]
    private void CmdSendCurDir(Vector2 v)
    {
        RpcSendCurDir(v);
    }

    [ClientRpc]
    private void RpcSendCurDir(Vector2 v)
    {
        CurTankDir = v;
    }



    public override void Died()
    {
        base.Died();
        Destroy(gameObject);
    }
}
