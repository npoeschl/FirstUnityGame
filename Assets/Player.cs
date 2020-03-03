using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject ShootingPoint;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private float gravity = 20f;
    private float vSpeed = 0f;
    bool canJump = true;
    bool canShoot = true;
    public float fireRate = 2f;
    public float nextFire;
    


    // Start is called before the first frame update
    void Start()
    {
       
    }

    void FixedUpdate()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        // Move Player around
        Vector3 movement = new Vector3(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal")) * moveSpeed;
        
        if (characterController.isGrounded)
        {
            vSpeed = 0;
            canJump = true;
            if (Input.GetButtonDown("Jump")) 
            {
                vSpeed = jumpSpeed;
            }

        }
        else
        {
            if (Input.GetButtonDown("Jump") && canJump)
            {
                vSpeed = jumpSpeed;
                canJump = false;
            }
        }
        vSpeed -= gravity * Time.deltaTime;
        movement.y = vSpeed;
        characterController.Move(movement * Time.deltaTime);


        // Shoot if you can
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            Instantiate(ProjectilePrefab, ShootingPoint.transform.position, Quaternion.identity);
            nextFire = Time.time + 1/fireRate;
        }
    }
}
