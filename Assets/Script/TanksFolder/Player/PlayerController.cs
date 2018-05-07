using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : TankController {

    private TankShoot ts;

    private void Start()
    {
        ts = GetComponent<TankShoot>();
        InitializationTank();
    }

    private void FixedUpdate()
    {

        MoveControll();
        SoundConroller();
        if (Input.GetKeyDown(KeyCode.Space) && ts.BulletIsReady())
            ts.ClientFire();

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


        if (hor != 0)
        {
            if (hor < 0)
            {
                CurrentTurn = TurnPosition.Right;
            }
            else
            {
                CurrentTurn = TurnPosition.Left;
            }

            Move();

        }

        if (ver != 0)
        {
            if (ver < 0)
            {
                CurrentTurn = TurnPosition.Down;
            }
            else
            {
                CurrentTurn = TurnPosition.Up;
            }

            Move();
        }




    }

    public override void Died()
    {
        base.Died();
        Destroy(gameObject);
    }
}

