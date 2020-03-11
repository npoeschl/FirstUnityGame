using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveProjectile : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 50.0F;
    public float damage;
    public float velocity;
    public Rigidbody rb;
    public GameObject impactParticles;

    void Awake()
    {
        //make projectile move
        Vector3 projectileForce = transform.forward * velocity;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(projectileForce, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Spawn Impact Particles
        Instantiate(impactParticles, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal, Vector3.up));
        // Trigger Explosion for all hit objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.gameObject != this.gameObject)
            {
                Debug.Log(rb.gameObject.name);
                rb.AddExplosionForce(power, transform.position, radius, 3.0F);
            }
                
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
