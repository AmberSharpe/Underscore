using UnityEngine;

enum State
{
    Solid,
    Liquid,
    Gas
}

public class PlayerController : MonoBehaviour
{
    private bool isGrounded
    {
        get
        {
            Vector2 boxSize = new Vector2(boxCollider2d.size.x, 0.05f);

            int boxOverlap = Physics2D.OverlapBox(
                new Vector2(transform.position.x, transform.position.y) + Vector2.down * (boxCollider2d.bounds.extents.y + boxSize.y),
                boxSize,
                0f,
                new ContactFilter2D()
                {
                    layerMask = LayerMask.GetMask("Ground"),
                    useLayerMask = true
                },
                new Collider2D[1]);

            return boxOverlap > 0;
        }
    }

    //References
    [SerializeField]
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;


    //State
    [SerializeField]
    State currentState = State.Solid;

    //Gravity
    public float gravity = -10;

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
        boxCollider2d = GetComponent<BoxCollider2D>();
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
            case State.Solid://Solid                              
                moveDirection = (Vector3.right * horizontalInput).normalized;
                moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput));

                if (jump && isGrounded)
                {
                    rb.velocity += Vector2.up * rb.mass * Mathf.Sqrt(2 * Mathf.Abs(gravity) * solidJumpHeight);
                }

                //Apply forces
                rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;
                rb.velocity += Vector2.up * gravity * Time.deltaTime;

                //Clamp Speed
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x, -solidMaxHorizontalSpeed, solidMaxHorizontalSpeed),
                    Mathf.Clamp(rb.velocity.y, -solidMaxVerticalSpeed, solidMaxVerticalSpeed)
                    );
                break;

            case State.Liquid: //Liquid                                      
                moveDirection = (Vector3.right * horizontalInput).normalized;
                moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput));

                //Apply force
                rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;

                //Clamp Speed
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x, -liquidMaxHorizontalSpeed, liquidMaxHorizontalSpeed),
                    Mathf.Clamp(rb.velocity.y, -liquidMaxVerticalSpeed, liquidMaxVerticalSpeed)
                    );
                break;

            case State.Gas://Gas                               
                moveDirection = (Vector3.right * horizontalInput + Vector3.up * verticalInput).normalized;
                moveDirectionMag = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

                //Apply force
                rb.velocity += baseMoveAcceleration * moveDirection * moveDirectionMag * Time.deltaTime;

                //Clamp Speed
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x, -gasMaxHorizontalSpeed, gasMaxHorizontalSpeed),
                    Mathf.Clamp(rb.velocity.y, -gasMaxVerticalSpeed, gasMaxVerticalSpeed)
                    );
                break;
        }
    }
}
