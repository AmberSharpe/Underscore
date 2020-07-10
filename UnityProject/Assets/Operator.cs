using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Operator : MonoBehaviour
{

    float verticalInput;
    float horizontalInput;                     
    float jumpBuffer = 0;                
    bool crouching;
    float flightThrustInput;                   

    Vector3 surfaceNormal;
    Vector3 moveDirection;
    float moveDirectionMag = 0;
    public Transform camTransform;

    void Start()
    {
        camTransform = FindObjectOfType<Camera>().transform;
        surfaceNormal = Vector3.up;
    }
   
    void Update()
    {
        CollectInput();            
    }

    private void FixedUpdate()
    {
        
    }

    private void CollectInput()
    {
        //Directional Input      
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    
        moveDirectionMag = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        //Jump                                
        if (Input.GetButtonDown("Jump"))
            jumpBuffer = 0;

        //Crouching            
        crouching = Input.GetButton("Crouching");

        if (Input.GetButton("Jump"))
            flightThrustInput = 1;
        else
            flightThrustInput = 0;

        ////Boost             
        //if (Input.GetAxisRaw("Fire3") == 1)
        //{
        //    if (boost_ResponceDisabled == false)
        //    {
        //        boostBuffer = 0;
        //        boost_ResponceDisabled = true;
        //    }
        //}
        //else
        //    boost_ResponceDisabled = false;

        //if (Input.GetAxisRaw("Fire3") == 1 && moveDirectionMagPrevious == 0)
        //    boostBuffer = 0;

        if (moveDirectionMag > 0)
        {
            Vector3 directionalForward = Vector3.Lerp(camTransform.forward, -camTransform.up, Vector3.Dot(camTransform.forward, surfaceNormal));
            moveDirection = ((directionalForward - (surfaceNormal * Vector3.Dot(directionalForward, surfaceNormal))).normalized * verticalInput +
            (camTransform.right - (surfaceNormal * Vector3.Dot(camTransform.right, surfaceNormal))).normalized * horizontalInput).normalized;
        }
        else
            moveDirection = Vector3.zero;
    }

}
