using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2d;
    private Animator anim;

    public float moveSpeed;
    public float jumpSpeed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool isGrounded;

    public Vector3 respawnPosition;
    public LevelManager levelManager;

    void Start () {

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        respawnPosition = transform.position;
        levelManager = FindObjectOfType<LevelManager>();
    }
	
	void Update () {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        Jump();
        Movement();

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

    // Kill Plane Collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillPlane")
        {
            levelManager.Respawn();
        }

        if (other.tag == "Checkpoint")
        {
            respawnPosition = other.transform.position;
        }
    }

    // Moving Platform Interaction
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }
}
