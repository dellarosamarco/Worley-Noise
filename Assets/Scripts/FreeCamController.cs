using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;

    [Header("Components")]
    public Transform orientation;

    private Vector3 moveDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");

        if (Input.GetKey("space"))
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("left shift"))
        {
            transform.position += -transform.up * moveSpeed * Time.deltaTime;
        }

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
    }

}
