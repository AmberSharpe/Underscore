using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    Rigidbody2D rb;

    Vector2 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        rb.velocity += (targetPos - (Vector2)transform.position).normalized * 2 * (targetPos - (Vector2)transform.position).magnitude;


    }
}
