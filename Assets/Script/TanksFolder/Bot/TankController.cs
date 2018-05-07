using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankController :NetworkBehaviour  {

    
    [Header("Properties")]
    [Range(1, 10)]
    public int speed = 5;


    [Header("Particle Components")]
    public ParticleSystem explosion;

    private TurnPosition curTurn = TurnPosition.Down;

    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public BoxCollider col;
    private AudioSource audio;

    public void InitializationTank()
    {
        col = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        audio = gameObject.GetComponent<AudioSource>();
        Turn(TurnPosition.Down);
    }

    private void Turn(TurnPosition pos)
    {
        switch (pos)
        {
            case (TurnPosition.Left):
                {
                    curTurn = TurnPosition.Left;
                    transform.localEulerAngles = new Vector3(0, 90, 0);
                    break;
                }

            case (TurnPosition.Right):
                {
                    curTurn = TurnPosition.Right;
                    transform.localEulerAngles = new Vector3(0, 270, 0);
                    break;
                }

            case (TurnPosition.Up):
                {
                    curTurn = TurnPosition.Up;
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                    break;
                }

            case (TurnPosition.Down):
                {
                    curTurn = TurnPosition.Down;
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                    break;
                }

        }
    }

    public void Move()
    {
        rb.velocity = transform.forward * speed;
    }



    public  void SoundConroller()
    {
        if (Mathf.Abs(rb.velocity.z) + Mathf.Abs(rb.velocity.x) < 2)
        {
            audio.pitch = Mathf.Clamp(audio.pitch -= 0.1f, 1, 2);

        }
        else if(Mathf.Abs(rb.velocity.z) + Mathf.Abs(rb.velocity.x) > 2)
        {

            audio.pitch = Mathf.Clamp(audio.pitch += 0.1f, 1, 2);
        }
    }

    public TurnPosition CurrentTurn
    {
        get { return curTurn; }
        set { Turn(value); }
    }

    public enum TurnPosition { Left , Right , Up, Down };

    virtual public void Died()
    {
        CreateParticleSys();
    }

    public void CreateParticleSys()
    {
        ParticleSystem ps = Instantiate(explosion, transform.position, transform.rotation);
        ps.Play();
        Destroy(ps.gameObject, ps.duration -2f);
    }
}
