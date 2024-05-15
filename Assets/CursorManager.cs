using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private float maxAngle = 30;
    [SerializeField] private float SmoothTime = 0.3f;

    private GameObject cursor;
    private Vector3 lastPoint;
    private Vector3 finalPoint;
    private GameObject player;
    private float playAngle;
    private Vector3 velocity = Vector3.zero;
    Plane plane = new Plane(Vector3.up, 0);
    void Awake()
    {
        cursor = gameObject;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playAngle = 2 * (90 - maxAngle);
    }

    // Update is called once per frame
    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            lastPoint = ray.GetPoint(distance);
            AngleControl();
        }
    }

    private void AngleControl()
    {
        Vector3 pointVector = lastPoint - player.transform.position;
        float angle = Vector3.Angle(pointVector, player.transform.forward);
        Debug.Log(angle);

        if(angle >= maxAngle && angle <= (maxAngle + playAngle))
        {
            finalPoint = lastPoint;
        }
        else
        {
/*            float dst = Vector3.Distance(lastPoint, player.transform.position);

            if(lastPoint.x > player.transform.position.x)
            {
                if(angle < maxAngle)
                {
                    finalPoint = new Vector3(dst * Mathf.Cos(maxAngle), 0, dst * Mathf.Sin(maxAngle));
                }
                else
                {
                    finalPoint = new Vector3(dst * Mathf.Cos(maxAngle), 0, dst * Mathf.Sin(maxAngle));
                }
            }
            else
            {
                if (angle < maxAngle)
                {
                    finalPoint = new Vector3(dst * Mathf.Cos(180 + maxAngle+playAngle), 0, dst * Mathf.Sin(180 + maxAngle + playAngle));
                }
                else
                {
                    finalPoint = new Vector3((-player.transform.right - maxAngle) * dst, 0, -player.transform.right.x * dst);
                }
            }*/
        }
        
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = finalPoint;
        cursor.transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }

    public Vector3 GetLastFinalPoint()
    {
        return finalPoint;
    }
}
