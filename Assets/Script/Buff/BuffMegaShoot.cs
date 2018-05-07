using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMegaShoot : Buff {

    [Header("Buff properties")]
    [Range(1, 5)]
    public int countShoot = 3;
    
    private TankShoot ts;
    private float normalTimeToShoot;
    new bool isLocalPlayer;
    public void Start()
    {
        isLocalPlayer = transform.parent.Find("Canvas").gameObject.activeSelf;
        ts = transform.parent.GetComponent<TankShoot>();
        normalTimeToShoot = ts.timeToReload;
        CalculateStep();
    }


    public void FixedUpdate()
    {

        if (countShoot == 0)
        {
            Destroy(newImg.gameObject);
            Destroy(gameObject);
        }
        ActiveBuff();
    }

    public override void CalculateStep()
    {
        step = 100/countShoot;
        step /= 100;

        Debug.Log(step);
    }

    public override void ActiveBuff()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isLocalPlayer)
        {
            
            ts.timeToReload = 0.3f;
            StartCoroutine(ITripleShoot());
        }
    }
    
    public IEnumerator ITripleShoot()
    {
        UpdateImage();

        yield return new WaitForSeconds(0.35f);
        for (int i = 0; i <2; i++)
        {

            ts.BulletIsReady();
            ts.ClientFire();            
            yield return new WaitForSeconds(0.35f);
        }
        ts.timeToReload = normalTimeToShoot;
        countShoot--;


    }
}
