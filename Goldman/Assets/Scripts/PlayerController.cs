using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    // Setup Variables //
    private Rigidbody2D rb2d;
    private Animator anim;

    public Vector3 respawnPosition;
    public LevelManager levelManager;
    public SpriteRenderer playerSprite;

    // Side Collisions //
    public Transform leftCollider1;
    public Transform leftCollider2;
    public Transform leftCollider3;
    public Transform leftCollider4;
    public Transform leftCollider5;

    public Transform rightCollider1;
    public Transform rightCollider2;
    public Transform rightCollider3;
    public Transform rightCollider4;
    public Transform rightCollider5;

    public Transform aboveCollider1;
    public Transform aboveCollider2;
    public Transform aboveCollider3;

    public Transform belowCollider1;
    public Transform belowCollider2;
    public Transform belowCollider3;

    public float climbRadius;
    public LayerMask whatIsClimbable;

    public bool climbableLeft;
    public bool climbableRight;

    //public bool colAbove;
    //public bool colBelow;

    public bool sticking;
    public bool canStick;
    public float clingTime;

    // Movement Variables //
    public float moveSpeed;
    public float jumpSpeed;

    public float jumpForceX;
    public float jumpForceY;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;

    public float invincibilityLength;
    private float invincibilityCounter;

    public bool flashActive;
    public float flashLength;                   // Should always be the same as invincibilityLength.
    private float flashCounter;

    void Start () {

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        respawnPosition = transform.position;
        levelManager = FindObjectOfType<LevelManager>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update () {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (knockbackCounter <= 0)
        {
            WallJump();
            Jump();
            Movement();

            if (invincibilityCounter <= 0)
            {
                levelManager.invincible = false;
            }
            if (flashCounter <= 0)
            {
                flashActive = false;
            }
        }

        FlashSprite();

        // Invincibility Timer
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
        }

        // Knockback Timer
        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;

            if (transform.localScale.x > 0)
                rb2d.velocity = new Vector3(-knockbackForce, knockbackForce, 0);
            if (transform.localScale.x < 0)
                rb2d.velocity = new Vector3(knockbackForce, knockbackForce, 0);
        }

    }

    public void WallJump()
    {
        if (Physics2D.OverlapCircle(leftCollider1.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(leftCollider2.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(leftCollider3.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(leftCollider4.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(leftCollider5.position, climbRadius, whatIsClimbable))
        {
            climbableLeft = true;
        }
        else
            climbableLeft = false;

        if (Physics2D.OverlapCircle(rightCollider1.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(rightCollider2.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(rightCollider3.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(rightCollider4.position, climbRadius, whatIsClimbable) ||
            Physics2D.OverlapCircle(rightCollider5.position, climbRadius, whatIsClimbable))
        {
            climbableRight = true;
        }
        else
            climbableRight = false;

        // Flag: If the player can stick onto the wall.
        if ((climbableLeft || climbableRight) && !isGrounded)
        {
            canStick = true;
        }

        //
        /*
        if (Input.GetButtonDown("Jump") && sticking)
        {
            rb2d.AddForce(new Vector2(jumpForceX, jumpForceY));
        }
        */
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, jumpSpeed, 0f);
        }
    }

    public void Movement()
    {
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            rb2d.velocity = new Vector3(moveSpeed, rb2d.velocity.y, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            rb2d.velocity = new Vector3(-moveSpeed, rb2d.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0f);
        }
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLength;
        invincibilityCounter = invincibilityLength;
        flashCounter = flashLength;

        levelManager.invincible = true;
        flashActive = true;
    }

    public void FlashSprite()
    {
        if (flashActive)
        {
            StartCoroutine("FlashSpriteCo");
            InvokeRepeating("FlashSpriteCo", flashLength, .1f);
        }
    }

    public IEnumerator FlashSpriteCo()
    {
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
        yield return new WaitForSeconds(.05f);
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
        yield return new WaitForSeconds(.05f);
    }

    public void Animation()
    {
        //anim.SetFloat("MoveSpeed", Mathf.Abs(rb2d.velocity.x));
        //anim.SetBool("Grounded", isGrounded);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kill Plane Collision
        if (other.tag == "KillPlane")
        {
            levelManager.Respawn();
        }

        if (other.tag == "Checkpoint")
        {
            respawnPosition = other.transform.position;
        }
    }

    // *OVERHAUL MOVING PLATFORM INTERACTION
    void OnCollisionEnter2D(Collision2D other)
    {
        // Moving Platform Interaction
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }
    // ''
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }
}
