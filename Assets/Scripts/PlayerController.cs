using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Vector2 moveDirection;
    private bool jump;
    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    private Rigidbody2D rb;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {


        rb.velocity += moveDirection * speed * Time.deltaTime;

        if (jump)
        {
            Debug.Log("Jump!");
        }

    }
}
