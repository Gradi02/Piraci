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





        if (angle >= maxAngle && angle <= maxAngle+playAngle)
        {
            finalPoint = lastPoint;
        }
        else
        {
            float dst = Vector3.Distance(lastPoint, player.transform.position);
            Vector3[] poss = new Vector3[4];

            //Vector3 direction = Quaternion.AngleAxis(maxAngle, Vector3.up) * player.transform.right;
            //Vector3 targetPosition = player.transform.position + direction.normalized * dst;

            poss[0] = player.transform.position + Quaternion.AngleAxis(maxAngle, Vector3.up) * player.transform.right * dst;
            poss[1] = player.transform.position + Quaternion.AngleAxis(maxAngle+playAngle, Vector3.up) * player.transform.right * dst;
            poss[2] = player.transform.position + Quaternion.AngleAxis(maxAngle, Vector3.up) * -player.transform.right * dst;
            poss[3] = player.transform.position + Quaternion.AngleAxis(maxAngle+playAngle, Vector3.up) * -player.transform.right* dst;

            Vector3 closest = poss[0];
            for(int i=1; i<4; i++)
            {
                if(Vector3.Distance(lastPoint, closest) > Vector3.Distance(lastPoint, poss[i]))
                {
                    closest = poss[i];
                }
            }

            finalPoint = closest;
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
