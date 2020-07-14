using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

enum state
{
    Solid,Liquid,Gas
}

public class PlayerController : MonoBehaviour
{
    //References
    [SerializeField]
    private Rigidbody2D rb;

    //State
    state currentState = state.Solid;

    //Limits
    public float solidJumpHeight = 10;

    const float solidMaxHorizontalSpeed = 10;
    const float solidMaxVerticalSpeed = 10;    
    
    const float liquidMaxHorizontalSpeed = 10;
    const float liquidMaxVerticalSpeed = 10;    

    const float gasMaxHorizontalSpeed = 10;
    const float gasMaxVerticalSpeed = 10;
    
    //Input
    private bool jump;
    private float horizontalInput;
    private float verticalInput;

    //Movement
    private float moveDirectionMag;        
    public Vector2 moveDirection;
    public float baseMoveAcceleration = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CollectInput();
    }

    private void CollectInput()
    {
        jump = Input.GetButtonDown("Jump");

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

    switch (currentState)
    {                
            case state.Solid://Solid                              
                moveDirection = (Vector3.right * horizontalInput).normalized;
                moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput));

                if (jump)
                {
                    Debug.Log("Jump!");
                    rb.velocity += Vector2.up * rb.mass * Mathf.Sqrt(2 * Physics.gravity.magnitude * solidJumpHeight);
                }

                //Apply force
                rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;
               
                //Clamp Speed
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x,-solidMaxHorizontalSpeed,solidMaxHorizontalSpeed), 
                    Mathf.Clamp(rb.velocity.y,-solidMaxVerticalSpeed,solidMaxVerticalSpeed)
                    );
                break;     

            case state.Liquid: //Liquid                                      
                moveDirection = (Vector3.right * horizontalInput).normalized;
                moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput));

                //Apply force
                rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;

                //Clamp Speed
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x,-liquidMaxHorizontalSpeed, liquidMaxHorizontalSpeed), 
                    Mathf.Clamp(rb.velocity.y,-liquidMaxVerticalSpeed, liquidMaxVerticalSpeed)
                    );
                break;      
                
            case state.Gas://Gas                               
                moveDirection = (Vector3.right * horizontalInput + Vector3.up * verticalInput).normalized;
                moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

                //Apply force
                rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;

                //Clamp Speed
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x,-gasMaxHorizontalSpeed,gasMaxHorizontalSpeed), 
                    Mathf.Clamp(rb.velocity.y,-gasMaxVerticalSpeed,gasMaxVerticalSpeed)
                    );
                break;
    }
               

    }
}
