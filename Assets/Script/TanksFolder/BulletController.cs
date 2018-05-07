using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public string tagTarget;
    public float speed =0.15f;
    public GameObject particle;
    public Rigidbody rb;
    void Start () {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
        

    }
	

	void FixedUpdate () {

        transform.position += transform.forward * speed;
	}
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.collider.tag)
        {
            case ("Bullet"):
            case ("BrickWall"):
                {
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                    break;
                }
            case ("Wall"):
                {
                    Destroy(gameObject);
                    break;
                }
            default:
                {
                    if(tagTarget == collision.collider.tag)
                    {
                        collision.gameObject.GetComponent<TankController>().Died();
                    }
                    Destroy(gameObject);
                    break;
                }
        }

    }


}
