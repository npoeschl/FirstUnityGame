using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject ShootingPoint;
    public GameObject Healthbar;
    public Animator anim;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private float gravity = 15f;
    private float vSpeed = 0f;
    bool canJump = true;
    public float fireRate = 2f;
    float nextFire;
    public float maxhealth = 100f;
    float currentHealth;
    Vector2 movementInput;


    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Run Forward", false);
        currentHealth = maxhealth;
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnShoot()
    {
        Shoot();
    }

    void OnJump()
    {
        Jump();
    }

    void Move()
    {
        // Store input values
        Vector3 movement = new Vector3(movementInput.x, 0.0f, movementInput.y);

        // Calculate downward speed with gravity
        vSpeed -= gravity * Time.deltaTime;
        movement.y = vSpeed;

        // Make Charactercontroller move the player
        // TODO: Make Jumpspeed independent from moveSpeed 
        characterController.Move(movement * Time.deltaTime * moveSpeed);

        // Rotate character only if input is given and set walking animation
        if (movement.x != 0 || movement.z != 0)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(movementInput.x, 0.0f, -movementInput.y));
            anim.SetBool("Run Forward", true);
        }
        // No Movement Input -> Idle
        else
        {
            anim.SetBool("Run Forward", false);
        }
    }

    void TakeDamage(float dmg)
    {
        if (currentHealth > 0)
        {
            currentHealth -= dmg;
            Healthbar.transform.localScale = new Vector3(currentHealth / maxhealth, 1, 1);
            Healthbar.transform.localPosition += new Vector3(-(dmg / maxhealth) / 2, 0, 0);
            if (currentHealth < 70)
            {
                Healthbar.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.yellow);
            }
            if (currentHealth < 50)
            {
                Healthbar.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.magenta);
            }
            if (currentHealth < 30)
            {
                Healthbar.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
            }
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("Die");
    }

    void Shoot()
    {
        anim.SetTrigger("Attack 01");
        Instantiate(ProjectilePrefab, ShootingPoint.transform.position, Quaternion.identity);
        nextFire = Time.time + 1 / fireRate;
        TakeDamage(10);
    }

    void Jump()
    {
        Debug.Log("Jump triggered");

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
        Move();
    }
}
