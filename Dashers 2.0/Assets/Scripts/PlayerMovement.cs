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
    bool isGrounded = false;
    bool isDashing;
    Rigidbody2D rb;
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Player";
    }

    
    void Update()
    {
        hMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && hMove != 0)
        {
            isDashing = true;
            CurrentDashTimer = StartDashTimer;
            rb.velocity = Vector2.zero;
            DashDirection = (int)hMove;
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
}
