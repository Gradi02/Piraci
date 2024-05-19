using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float rotationDirection;
    private Transform player;
    private int maxDst = 12;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        rb.useGravity = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maxDst = Random.Range(6, 12);
        moveSpeed += Random.Range(-0.8f, 0);
    }

    void Update()
    {
        SpeedAndRotationControl();
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

    private void FixedUpdate()
    {
        MoveEnemy();
        RotateEnemy();
    }

    private void MoveEnemy()
    {
        // calculate movement direction
        float d = Vector3.Distance(player.position, transform.position);
        Debug.Log(d);

        if(d<maxDst)
        {
            moveDirection = RotateVector90Degrees((player.position - transform.position).normalized);
            if (Vector3.Angle(transform.right, moveDirection) < Vector3.Angle(-transform.right, moveDirection))
            {
                rotationDirection = Random.Range(0.8f, 1);
            }
            else
            {
                rotationDirection = Random.Range(-1, -0.8f);
            }
        }
        else
        {
            moveDirection = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.right, moveDirection) < Vector3.Angle(-transform.right, moveDirection))
            {
                rotationDirection = Random.Range(0.8f, 1);
            }
            else
            {
                rotationDirection = Random.Range(-1, -0.8f);
            }
        }

        // add force
        rb.AddForce(transform.forward * moveSpeed / 2, ForceMode.Force);
    }

    private void RotateEnemy()
    {
        if (rotationDirection != 0 && rb.velocity.magnitude > 0.5f)
        {
            rb.AddTorque(new Vector3(0, rotationDirection * rotationSpeed, 0), ForceMode.Force);
        }
    }

    private Vector3 RotateVector90Degrees(Vector3 vector)
    {
        // Obrót o 90 stopni w prawo (zgodnie z ruchem wskazówek zegara)
        return new Vector3(vector.z, vector.y, -vector.x);
    }
}
