using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Hazards hazard;
    
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
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButtonDown("Jump") && isGrounded) // bools default to true in an if statement so you dont need == true here
        {
            jump = true;
            isGrounded = false;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && hMove != 0 && dashCount < maxDash)
        {
            isDashing = true;
            CurrentDashTimer = StartDashTimer;
            rb.velocity = Vector2.zero;
            DashDirection = (int)hMove;
            dashCount = dashCount + 1;
        }
        if (isDashing)
        {
            rb.velocity = transform.right * DashDirection * DashForce;
            CurrentDashTimer -= Time.deltaTime;
            if(CurrentDashTimer <= 0)
            {
                isDashing = false;
            }
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
}
