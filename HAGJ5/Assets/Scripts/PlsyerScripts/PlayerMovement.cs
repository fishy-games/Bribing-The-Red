using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //movement
    public float speed = 5;
    private float moveInput;

    private Rigidbody2D rb;

    //looks
    private bool facingRight = true;
    private Animator anim;

    //jump
    public float jumpForce = 5f;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    public int extraJumps = 2;
    private int amtOfJumps;

    //hide /crouch
    public LayerMask whatIsCover;
    private bool isInCover = false;
    private BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        amtOfJumps = extraJumps;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);



        if (moveInput == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }


        isInCover = Physics2D.OverlapCircle(transform.position, 1f, whatIsCover); //hiding
        if (Input.GetKey(KeyCode.C) && (isInCover)) //crouch or hide
        {
            //disable collider
            col.enabled = false;
            
            //activate mask where hiding or smth
        }
        else 
        {
            col.enabled = true;
        }
    }

    private void Update()
    {
        if (isGrounded == true)
        {
            amtOfJumps = extraJumps;
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
        }


        if (amtOfJumps>0 && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            anim.SetTrigger("takeOff");

            rb.velocity = Vector2.up * jumpForce;
            amtOfJumps--;
        }
        else if (isGrounded == true && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
        
    }
}
