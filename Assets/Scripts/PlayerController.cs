using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[SerializeField]

public class PlayerController : MonoBehaviour
{
    //References
    [SerializeField]
    private Rigidbody2D rb;

    private Animator animator;

    [SerializeField]
    private PhysicsMaterial2D PhyMat2D;
    private LayerMask mask;

    //State
    [SerializeField]
    int currentMassState = 0;
    public bool grounded = false;

    //GroundCheck
    public float groundCheckWidth = 8;

    //Gravity
    public float gravity = -100;

    //Character
    public float massChangeRate = 3;
    public float jumpHeight = 128;

    const float maxHorizontalSpeed = 200;
    const float maxVerticalSpeed = 100;    
    
    //Input
    public bool playerDriven = false;
    private float jumpBuffer;
    private float jumpBufferMax = .5f;
    private float horizontalInput;
    private float verticalInput;

    //Movement
    private float moveDirectionMag;        
    public Vector2 moveDirection;
    public float baseMoveAcceleration = 1000;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        PhyMat2D = rb.sharedMaterial;
        mask = ~LayerMask.NameToLayer("Player");
        jumpBuffer = jumpBufferMax;
    }

    private void Update()
    {
        CollectInput();

        rb.mass = Mathf.Clamp(rb.mass + verticalInput * massChangeRate * Time.deltaTime, 1, 10);

        currentMassState = Mathf.RoundToInt(rb.mass - 1);


        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetInteger("State", currentMassState);
    }

    private void CollectInput()
    {
        //Update Jump Buffer
        jumpBuffer += Time.deltaTime;

        if (playerDriven)
        {
            if (Input.GetButtonDown("Jump"))
                jumpBuffer = 0;

            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
    }
    private void LateUpdate()
    {
        //Grounded Check        
        if (CheckIfGroundIsBelow())
            grounded = true;
        else
            grounded = false;
    }

    void FixedUpdate()
    {

        if (playerDriven)
            Movement();

    }


    private bool CheckIfGroundIsBelow()
    {

        for (int i = -1; i <= 1; i++)
        {
            Vector2 pos = transform.position + (i * Vector3.right * groundCheckWidth/2) - Vector3.up * 16;
                  
            Debug.DrawRay(pos, Vector2.down * 2);
        
            if (Physics2D.Raycast(pos, Vector2.down, 2, ~(1 << 8)))
                return true;
        }

        return false;

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));
        rb.velocity += Vector2.up * Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHeight);
        jumpBuffer = jumpBufferMax;
        grounded = false;
    }

    private void Movement()
    {


        moveDirection = (Vector3.right * horizontalInput).normalized;
        moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput));

        if (jumpBuffer < jumpBufferMax && grounded)
            Jump();

        //Apply forces
        rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;
        rb.velocity += Vector2.up * gravity * (rb.mass * .5f) * Time.deltaTime;


        if (moveDirectionMag == 0 && grounded)
        {
            if (Mathf.Abs(rb.velocity.x) > 64)
                rb.velocity -= Vector2.right * Mathf.Sign(rb.velocity.x) * baseMoveAcceleration * Time.deltaTime;
            else
                rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //Clamp Speed
        rb.velocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed),
            Mathf.Clamp(rb.velocity.y, -maxVerticalSpeed, rb.velocity.y)
            );

        //switch (currentState)
        //{                
        //    case State.Solid:                           
        //        moveDirection = (Vector3.right * horizontalInput).normalized;
        //        moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput));

        //        if (jumpBuffer < jumpBufferMax && grounded)                
        //            Jump();

        //        //Apply forces
        //        rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;
        //        rb.velocity += Vector2.up * gravity * Time.deltaTime;

        //        //Clamp Speed
        //        rb.velocity = new Vector2(
        //            Mathf.Clamp(rb.velocity.x,-maxHorizontalSpeed,maxHorizontalSpeed), 
        //            Mathf.Clamp(rb.velocity.y,-maxVerticalSpeed,maxVerticalSpeed)
        //            );
        //        break;     

        //    case State.Liquid: //Liquid                                      
        //        moveDirection = (Vector3.right * horizontalInput).normalized;
        //        moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput));

        //        //Apply force
        //        rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;

        //        //Clamp Speed
        //        rb.velocity = new Vector2(
        //            Mathf.Clamp(rb.velocity.x,-liquidMaxHorizontalSpeed, liquidMaxHorizontalSpeed), 
        //            Mathf.Clamp(rb.velocity.y,-liquidMaxVerticalSpeed, liquidMaxVerticalSpeed)
        //            );
        //        break;      

        //    case State.Gas://Gas                               
        //        moveDirection = (Vector3.right * horizontalInput + Vector3.up * verticalInput).normalized;
        //        moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        //        //Apply force
        //        rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;

        //        //Clamp Speed
        //        rb.velocity = new Vector2(
        //            Mathf.Clamp(rb.velocity.x,-gasMaxHorizontalSpeed,gasMaxHorizontalSpeed), 
        //            Mathf.Clamp(rb.velocity.y,-gasMaxVerticalSpeed,gasMaxVerticalSpeed)
        //            );
        //        break;
        //}


    }
}
