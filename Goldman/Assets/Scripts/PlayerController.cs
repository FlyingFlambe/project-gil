using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    // Class-Specific Variables //
    private Rigidbody2D rb2d;
    //private Animator anim;
    private SideCollision scs;

    public Vector3 respawnPosition;
    public LevelManager levelManager;
    public SpriteRenderer playerSprite;

    // Controls //
    public bool moveLeft;
    public bool moveRight;
    public bool inAirUp;
    public bool inAirDown;

    // Movement Variables //
    public float moveSpeed;
    public float jumpSpeed;
    public float gravNorm;                      // Normal fall gravity.
    public float gravSlide;                     // Gravity when sliding on a wall.

    public bool canStick;                       // If the player can stick onto a wall
    public bool isSticking;                     // If the player is currently sticking on a wall
    public bool postStick;                      // If the player decides to let go of a wall (typically in anticipation of a wall-jump)
    public bool canWallJump;
    public float gravCounter;
    public float clingTime;

    public float jumpForceX;
    public float jumpForceY;

    // Checks / Other //
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;

    public LayerMask whatIsClimbable;
    public bool leftClimbable;
    public bool rightClimbable;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;

    public float invincibilityLength;
    private float invincibilityCounter;

    public bool flashActive;
    public float flashLength;                   // Should always be the same as invincibilityLength.
    private float flashCounter;

    void Start () {

        // Set classes.
        rb2d = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        scs = GetComponent<SideCollision>();

        respawnPosition = transform.position;
        levelManager = FindObjectOfType<LevelManager>();
        playerSprite = GetComponent<SpriteRenderer>();

    }

    void Update () {

        //Debug.Log(Camera.main.pixelHeight);

        SetControls();

        // If there's no knockback from taking damage, run things as usual.
        if (knockbackCounter <= 0)
        {
            WallSlide();
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

        // Flash player sprite when damage is taken.
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

            if (playerSprite.flipX == false)
                rb2d.velocity = new Vector3(-knockbackForce, knockbackForce, 0);
            if (playerSprite.flipX == true)
                rb2d.velocity = new Vector3(knockbackForce, knockbackForce, 0);
        }

    }

    public void SetControls()
    {
        // Set Grounded variable
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Left Movement
        if (Input.GetAxisRaw("Horizontal") < 0f)
            moveLeft = true;
        else
            moveLeft = false;

        // Right Movement
        if (Input.GetAxisRaw("Horizontal") > 0f)
            moveRight = true;
        else
            moveRight = false;

        // Moving Upward (No Direct Control)
        if ((rb2d.velocity.y > 0) && !isGrounded)
            inAirUp = true;
        else
            inAirUp = false;

        // Moving Downward (No Direct Control)
        if ((rb2d.velocity.y < 0) && isGrounded)
            inAirDown = true;
        else
            inAirDown = false;
    }

    public void WallSlide()
    {
        // Set climbable variables back to false unless collisions with climbables occur.
        if (!scs.leftCollision || isGrounded)
            leftClimbable = false;
        if (!scs.rightCollision || isGrounded)
            rightClimbable = false;

        // Flag: If the player can stick onto the wall.
        if (leftClimbable || rightClimbable)
            canStick = true;
        else
            canStick = false;

        // Flag: If the player is currently sticking onto the wall.
        if ((leftClimbable && moveLeft) || (rightClimbable && moveRight))
        {
            clingTime = 0.2f;
            isSticking = true;
        }
        else
            isSticking = false;

        // Change falling speed when sliding on wall.
        if (isSticking && clingTime > 0f)
        {
            clingTime = 0.2f;

            if (rb2d.velocity.y < 0f)
                rb2d.gravityScale = gravSlide;
            else
            {
                rb2d.gravityScale = 0f;
                rb2d.velocity = new Vector3(rb2d.velocity.x, 0f, 0f);
            }
        }
        else if (!isSticking && clingTime > 0f && (leftClimbable || rightClimbable))
        {
            clingTime -= (Time.deltaTime);
            postStick = true;
        }
        else
        {
            rb2d.gravityScale = gravNorm;
            isSticking = false;
            postStick = false;
        }
    }

    public void Jump()
    {
        // Basic Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, jumpSpeed, 0f);
        }

        // Wall Jumping
        if (clingTime > 0f && clingTime < 0.2f)
            canWallJump = true;
        else
            canWallJump = false;

        if (canWallJump && postStick)
        {
            if (leftClimbable)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rb2d.velocity = new Vector3(jumpForceX, jumpForceY, 0f);
                    postStick = false;
                }
            }

            if (rightClimbable && postStick)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rb2d.velocity = new Vector3(-jumpForceX, jumpForceY, 0f);
                    postStick = false;
                }
            }
        }
    }

    public void Movement()
    {
        if (moveRight)
        {
            rb2d.velocity = new Vector3(moveSpeed, rb2d.velocity.y, 0f);
            playerSprite.flipX = false;
        }
        else if (moveLeft)
        {
            rb2d.velocity = new Vector3(-moveSpeed, rb2d.velocity.y, 0f);
            playerSprite.flipX = true;
        }
        else
            rb2d.velocity = new Vector3(0, rb2d.velocity.y, 0f);
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
        Collider2D collider = other.collider;
        
        // Moving Platform Interaction
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (scs.leftCollision && !isGrounded)
                leftClimbable = true;

            if (scs.rightCollision && !isGrounded)
                rightClimbable = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }
}
