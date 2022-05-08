using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //allows access to other scripts
    public CharacterController2D controller;
    public Hazards hazard;
    public PhaseWall phase;
    
    //basic movement variables
    float hMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;

    //variables for the dashing mechanics
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
    public int phaseDashCount = 0;
    public int phaseMaxDash = 1;

    //variables for the wall grab mechanic
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
        gravStore = rb.gravityScale;// store the current gravity setting
    }

    
    void Update()
    {
        
        if (wallJumpCounter <= 0)//allows the activation of the wall jump
        {
            hMove = Input.GetAxisRaw("Horizontal") * runSpeed; // basic 2d movement
            if (Input.GetButtonDown("Jump") && isGrounded) // bools default to true in an if statement so you dont need == true here
            {
                jump = true;
                isGrounded = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && hMove != 0 && dashCount < maxDash)//starts dash movement
            {
                isDashing = true;
                CurrentDashTimer = StartDashTimer;
                rb.velocity = Vector2.zero;
                DashDirection = (int)hMove;
                dashCount = dashCount + 1;
            }
            if (Input.GetKeyDown(KeyCode.E) && hMove != 0 && phaseDashCount < phaseMaxDash) //starts phase dash, 
            {
                    phase.Phase();// calls the phase mechanic from the phase script
                isDashing = true;
                CurrentDashTimer = StartDashTimer;
                rb.velocity = Vector2.zero;
                DashDirection = (int)hMove;
                phaseDashCount = phaseDashCount + 1;
            }
            if (isDashing)// moves the player
            {
                rb.velocity = transform.right * DashDirection * DashForce;
                CurrentDashTimer -= Time.deltaTime;
                if (CurrentDashTimer <= 0)// stops the dash from being infinite
                {
                    isDashing = false;
                }
            }

            canGrab = Physics2D.OverlapCircle(wallGrabPoint.position, 0.2f, whatIsGrabbable);// checks if there is anything to grab
            isGrabbing = false;
            if (canGrab && !isGrounded ) // chacks conditions and sets of grabbing
            {
                if ((transform.localScale.x == 1f && Input.GetAxisRaw("Horizontal") > 0) || (transform.localScale.x == -1f && Input.GetAxisRaw("Horizontal") < 0))
                {
                    isGrabbing = true;
                }
            }
            if (isGrabbing) //freezes the player where they are
            {
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
                if (Input.GetButtonDown("Jump"))// allows the player to jump away from the wall
                {
                    wallJumpCounter = wallJumpTime;
                    rb.velocity = new Vector2(-Input.GetAxisRaw("Horizontal") * runSpeed, newJump);
                    rb.gravityScale = gravStore;
                    isGrabbing = false;
                }
            }
            else
            {
                rb.gravityScale = gravStore;// restores gravity
            }
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;// reverts counter back to default
        }

    }

    private void FixedUpdate() // basic movement
    {
        controller.Move(hMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hazard")// triggers the respawn when you hit a hazard
        {
            hazard.Reload(gameObject);
        }
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))// resets the dash counters and constantly updates the ground check allowing the various mechanics
        if(collision.collider.CompareTag("Ground"))// resets the dash counters and constantly updates the ground check allowing the various mechanics
        {
            isGrounded = true;
            dashCount = 0;
            phaseDashCount = 0;
        }
    }
    //based on Brackeys movement Tutorial, gamesplusjames's WallJumping tutorial & Muddy Wolf's Dash Tutorial, modified to suit game needs
    //Muddy Wolf https://www.youtube.com/watch?v=yB6ty0Gj7tA
    //Brackey https://www.youtube.com/watch?v=dwcT-Dch0bA
    //gamesplusjames https://www.youtube.com/watch?v=uNJanDrjMgU
}
