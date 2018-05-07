using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSuperSpeed : Buff
{
    private int maxSpeed = 7;
    private int normalSpeed;
    private TankController tc;
    public void Start()
    {

        tc = transform.parent.GetComponent<TankController>();
        ActiveBuff();
        CalculateStep();

    }

    public void FixedUpdate()
    {
        UpdateImage();
    }
    public override void ActiveBuff()
    {
        
        normalSpeed = tc.speed;
        tc.speed = maxSpeed;
        StartCoroutine(ISuperSeed());


    }

    public IEnumerator ISuperSeed()
    {
        yield return new WaitForSeconds(actionTime);
        tc.speed = normalSpeed;
        Destroy(gameObject);
    }
}
