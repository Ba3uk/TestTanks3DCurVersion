using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankShoot : NetworkBehaviour {

    [Range(1, 5)]
    public float timeToReload = 1f;
    public string tagTarget;
    public ParticleSystem shootEffect;

    [SerializeField]
    private GameObject bulletPrefab;
    private AudioSource bulletSourse;

    
    private float timeLastShoot = 0;

    void Start()
    {
        bulletSourse = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public bool BulletIsReady()
    {
        return timeLastShoot < Time.fixedTime - timeToReload;
    }




    public void ClientFire()
    {
        if (BulletIsReady())
        {
            CmdTankShoot(transform.name, transform.GetChild(0).position, transform.rotation);
            timeLastShoot = Time.fixedTime;
        }

    }

    [Command]
    private void CmdTankShoot(string name , Vector3 _position, Quaternion _rotaion)
    {
        bulletSourse.Play();
        GameObject bullet = Instantiate(bulletPrefab, _position, _rotaion);
        bullet.GetComponent<BulletController>().tagTarget = tagTarget;
        NetworkServer.Spawn(bullet);
        RpcPlayParticle(_position, _rotaion);

    }

    [ClientRpc]
    private void RpcPlayParticle(Vector3 _position, Quaternion _rotaion)
    {
        ParticleSystem ps = Instantiate(shootEffect, _position + transform.forward * 0.5f, _rotaion);
        ps.Play();
        Destroy(ps.gameObject, ps.duration);
    }

}
