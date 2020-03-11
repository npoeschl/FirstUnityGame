using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public GameObject ShootingPoint;
    public GameObject Healthbar;
    public Animator anim;
    public UnityEngine.InputSystem.InputActionAsset inputAction;
    public UnityEngine.InputSystem.InputActionMap actionMap;
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
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    void Awake()
    {
        
        
    } 

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Run Forward", false);
        currentHealth = maxhealth;

        gradient = new Gradient();
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[3];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.yellow;
        colorKey[1].time = 0.5f;
        colorKey[2].color = Color.green;
        colorKey[2].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[3];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 0.5f;
        alphaKey[2].alpha = 1.0f;
        alphaKey[2].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
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
        //Vector3 movement = new Vector3(movementInput.y, 0.0f, -movementInput.x);
        Vector3 movement = new Vector3(0.0f, 0.0f, -movementInput.x);

        // Calculate downward speed with gravity
        vSpeed -= gravity * Time.deltaTime;
        movement.y = vSpeed;

        // Make Charactercontroller move the player
        // TODO: Make Jumpspeed independent from moveSpeed 
        characterController.Move(movement * Time.deltaTime * moveSpeed);

        // Rotate character only if input is given and set walking animation
        if (movement.x != 0 || movement.z != 0)
        {
            //transform.rotation = Quaternion.LookRotation(new Vector3(movementInput.y, 0.0f, -movementInput.x));
            transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, -movementInput.x));
            anim.SetBool("Run Forward", true);
        }
        // No Movement Input -> Idle
        else
        {
            anim.SetBool("Run Forward", false);
        }
    }

    public void TakeDamage(float dmg)
    {
        if (currentHealth > 0)
        {
            currentHealth = Mathf.Clamp(currentHealth-dmg, 0, maxhealth);
            // currentHealth -= dmg;
            UpdateHealthBar(dmg);
          
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
        if (Time.time >= nextFire)
        {
            anim.SetTrigger("Attack 01");
            GameObject bullet = Instantiate(ProjectilePrefab, ShootingPoint.transform.position, Quaternion.LookRotation(transform.forward));
            nextFire = Time.time + 1 / fireRate;
        }
        
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

    void UpdateHealthBar(float dmg)
    {
        Healthbar.transform.localScale = new Vector3(currentHealth / maxhealth, 1, 1);
        Healthbar.transform.localPosition += new Vector3(-(dmg / maxhealth) / 2, 0, 0);
        Healthbar.GetComponentInChildren<Renderer>().material.SetColor("_Color", gradient.Evaluate(currentHealth/maxhealth));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
