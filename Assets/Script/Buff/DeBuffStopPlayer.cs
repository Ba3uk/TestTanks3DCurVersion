using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffStopPlayer : Buff {
    private ClientPlayerControl cpc;
    private TankShoot ts;
    private Rigidbody rb;

    void Start ()
    {
        ts = transform.parent.GetComponent<TankShoot>();
        rb = transform.parent.GetComponent<Rigidbody>();
        cpc = transform.parent.GetComponent<ClientPlayerControl>();
        ActiveBuff();
        CalculateStep();

    }
    private void FixedUpdate()
    {
        UpdateImage();
    }

    public override void ActiveBuff()
    {
        rb.velocity = Vector3.zero;
        cpc.enabled = false;
        ts.enabled = false;
        StartCoroutine(FrizePlayer());
    }


    public IEnumerator FrizePlayer()
    {
        yield return new WaitForSeconds(actionTime);
        ts.enabled = true;
        cpc.enabled = true;
        Destroy(gameObject);
    }
}
