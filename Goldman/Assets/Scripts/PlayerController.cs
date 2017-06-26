using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    // Setup Variables //
    private Rigidbody2D rb2d;
    private Animator anim;

    public Vector3 respawnPosition;
    public LevelManager levelManager;
    public SideCollision sideCollision;

    // Side Collisions //
    public BoxCollider2D leftCollider;
    public BoxCollider2D rightCollider;
    public BoxCollider2D aboveCollider;
    public BoxCollider2D belowCollider;

    public bool colLeft;
    public bool colRight;
    public bool colAbove;
    public bool colBelow;

    // Movement Variables //
    public float moveSpeed;
    public float jumpSpeed;

    public float jumpForceX;
    public float jumpForceY;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    public bool sticking;
    public bool canStick;
    public float clingTime;

    void Start () {

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        respawnPosition = transform.position;
        levelManager = FindObjectOfType<LevelManager>();
        sideCollision = GetComponent<SideCollision>();

        leftCollider = GetComponent<BoxCollider2D>();
        rightCollider = GetComponent<BoxCollider2D>();
        aboveCollider = GetComponent<BoxCollider2D>();
        belowCollider = GetComponent<BoxCollider2D>();
    }

    void Update () {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        WallJump();
        Jump();
        Movement();

        //leftCollider.isTrigger.

    }

    void WallJump()
    {
        // Flag: If the player can stick onto the wall.
        /* if (!isGrounded && )
        {
            canStick = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        }

        // 
        if (Input.GetButtonDown("Jump") && sticking)
        {
            rb2d.AddForce(new Vector2(jumpForceX, jumpForceY));
        } */
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, jumpSpeed, 0f);
        }
    }

    void Movement()
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

    void Animation()
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
