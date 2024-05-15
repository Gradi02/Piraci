using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float power = 2;

    private Vector3 shotPosition;
    private PlayerMovement pm;
    private CursorManager cm;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        cm = GameObject.FindGameObjectWithTag("Cursor").GetComponent<CursorManager>();
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            shotPosition = cm.GetLastFinalPoint();
            GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody rb = b.GetComponent<Rigidbody>();
            rb.AddForce(CalculateTrajectoryVelocity(transform.position, shotPosition, power), ForceMode.Impulse);
            rb.AddTorque(pm.GetCurrentVelocity()*10, ForceMode.Impulse);
            rb.AddForce(pm.GetCurrentVelocity(), ForceMode.Impulse);
        }
    }

    private Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }
}
