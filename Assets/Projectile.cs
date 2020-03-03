using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float damage;
    public float velocity;
    public Rigidbody rb;
    public GameObject impactParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //make projectile move
        Vector3 projectileForce = new Vector3(0,0, velocity) ;
        rb.AddForce(projectileForce);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider);
        Instantiate(impactParticles, collision.contacts[0].point, Quaternion.identity);
        Destroy(gameObject);
    }
}
