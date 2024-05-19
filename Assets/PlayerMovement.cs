using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    public TextMeshProUGUI text;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        rb.useGravity = false;
    }


    private void Update()
    {
        MyInput();
        SpeedAndRotationControl();
        text.text = "vel: " + rb.velocity.magnitude + " | ang: " + rb.angularVelocity.magnitude;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = (Mathf.Abs(verticalInput)*transform.forward).normalized;

        // add force
        rb.AddForce(moveDirection * moveSpeed/2, ForceMode.Force);
    }

    private void MyInput()
    {
        horizontalInput = Mathf.Clamp(Input.GetAxisRaw("Horizontal") + Random.Range(-0.1f, 0.1f), -1, 1);
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void SpeedAndRotationControl()
    {
        // check for max speed
        Vector3 flatMove = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatMove.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatMove.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }



        // check for max angular velocity
        Vector3 flatRot = new Vector3(0f, rb.angularVelocity.y, 0f);

        if (flatRot.magnitude > rotationSpeed)
        {
            Vector3 limitedVel = flatRot.normalized * rotationSpeed;
            rb.angularVelocity = new Vector3(0f, limitedVel.y, 0f);
        }
    }

    private void RotatePlayer()
    {
        if (horizontalInput != 0 && rb.velocity.magnitude > 0.5f)
        {
            rb.AddTorque(new Vector3(0, horizontalInput * rotationSpeed, 0), ForceMode.Force);
        }
    }

    public Vector3 GetCurrentVelocity()
    {
        return rb.velocity;
    }
}
