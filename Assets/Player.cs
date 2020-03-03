using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject ShootingPoint;
    public Animator anim;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private float gravity = 15f;
    private float vSpeed = 0f;
    bool canJump = true;
    bool canShoot = true;
    public float fireRate = 2f;
    public float nextFire;
    

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Run Forward", false);
    }

    void Shoot()
    {
        anim.SetTrigger("Attack 01");
        Instantiate(ProjectilePrefab, ShootingPoint.transform.position, Quaternion.identity);
        nextFire = Time.time + 1 / fireRate;
    }

    void Jump()
    {
        // Set canJump true if player is grounded
        if (characterController.isGrounded)
        {
            canJump = true;
            // Do the jump
            vSpeed = jumpSpeed;
            anim.SetTrigger("Jump");
        }
        // Player is airborn
        else 
        {
            if (canJump)
            {
                // Do the doublejump
                vSpeed = jumpSpeed;
                canJump = false;
            }
        }
              
    }

    // Update is called once per frame
    void Update()
    {
        // Store input values
        Vector3 movement = new Vector3(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));
        Debug.Log(movement);
        
        // Try to jump
        if (Input.GetButtonDown("Jump")) 
        {
            Jump();
        }

        // Try to shoot
        if (Input.GetAxis("RightTrigger") > 0 && Time.time >= nextFire)
        {
            Shoot();
        }

        // Calculate downward speed with gravity
        vSpeed -= gravity * Time.deltaTime;
        movement.y = vSpeed;
        
        // Make Charactercontroller move the player
        // TODO: Make Jumpspeed independent from moveSpeed 
        characterController.Move(movement * Time.deltaTime * moveSpeed);
        
        // Rotate character only if input is given and set walking animation
        if (movement.x != 0 || movement.z != 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal")));
            anim.SetBool("Run Forward", true);
        }
        // No Movement Input -> Idle
        else
        {
            anim.SetBool("Run Forward", false);
        }
        
        
       
    }
}
