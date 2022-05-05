using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Hazards hazard;
    public PhaseWall phase;
    
    float hMove = 0f;
    public float runSpeed = 40f;
    
    bool jump = false;

    public float DashForce;
    public float StartDashTimer;
    float CurrentDashTimer;
    float DashDirection;
    public bool isGrounded = true;
    bool isDashing;
    Rigidbody2D rb;
    public int dashCount = 0;
    public int maxDash = 3;
    Collider2D col;

    public Transform wallGrabPoint;
    bool canGrab;
    bool isGrabbing;
    public float wallJumpTime = 0.2f;
    float wallJumpCounter;
    float gravStore;
    public float newJump = 2f;
    public LayerMask whatIsGrabbable;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        gravStore = rb.gravityScale;
    }

    
    void Update()
    {
        
        if (wallJumpCounter <= 0)
        {
            hMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            if (Input.GetButtonDown("Jump") && isGrounded) // bools default to true in an if statement so you dont need == true here
            {
                jump = true;
                isGrounded = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && hMove != 0 && dashCount < maxDash)
            {
                isDashing = true;
                CurrentDashTimer = StartDashTimer;
                rb.velocity = Vector2.zero;
                DashDirection = (int)hMove;
                dashCount = dashCount + 1;
            }
            if (Input.GetKeyDown(KeyCode.E) && hMove != 0 && dashCount < maxDash)
            {
                if (col.CompareTag("pass"))
                {
                    phase.Phase();
                    isDashing = true;
                    CurrentDashTimer = StartDashTimer;
                    rb.velocity = Vector2.zero;
                    DashDirection = (int)hMove;
                    dashCount = dashCount + 1;
                }
            }
            if (isDashing)
            {
                rb.velocity = transform.right * DashDirection * DashForce;
                CurrentDashTimer -= Time.deltaTime;
                if (CurrentDashTimer <= 0)
                {
                    isDashing = false;
                }
            }

            canGrab = Physics2D.OverlapCircle(wallGrabPoint.position, 0.2f, whatIsGrabbable);
            isGrabbing = false;
            if (canGrab && !isGrounded )
            {
                if ((transform.localScale.x == 1f && Input.GetAxisRaw("Horizontal") > 0) || (transform.localScale.x == -1f && Input.GetAxisRaw("Horizontal") < 0))
                {
                    isGrabbing = true;
                }
            }
            if (isGrabbing)
            {
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
                if (Input.GetButtonDown("Jump"))
                {
                    wallJumpCounter = wallJumpTime;
                    rb.velocity = new Vector2(-Input.GetAxisRaw("Horizontal") * runSpeed, newJump);
                    rb.gravityScale = gravStore;
                    isGrabbing = false;
                }
            }
            else
            {
                rb.gravityScale = gravStore;
            }
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        controller.Move(hMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hazard")
        {
            hazard.Reload(gameObject);
        }
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            dashCount = 0;
        }
    }
    //based on Brackeys movement Tutorial, gamesplusjames's WallJumping tutorial & Muddy Wolf's Dash Tutorial, modified to suit game needs
    //Muddy Wolf https://www.youtube.com/watch?v=yB6ty0Gj7tA
    //Brackey https://www.youtube.com/watch?v=dwcT-Dch0bA
    //gamesplusjames https://www.youtube.com/watch?v=uNJanDrjMgU
}
