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
    void Awake()
    {
        //make projectile move
        Vector3 projectileForce = transform.forward * velocity;
        rb.AddForce(projectileForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider);
        Instantiate(impactParticles, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal, Vector3.up));
        // Damage Players
        Player hitPlayer = collision.gameObject.GetComponent<Player>();
        if(hitPlayer != null)
        {
            hitPlayer.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
