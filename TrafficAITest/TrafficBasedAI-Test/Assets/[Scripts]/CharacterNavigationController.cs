using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{
    public Transform mTransform;

    public Vector3 destination;

    public float stopDistance = .5f;
    public float movementSpeed = 1f;
    public float rotationSpeed = 120f;

    public bool reachedDestination = false;

    [HideInInspector] public Vector3 lastPosition;

    [Header("Sensors")]
    public float sensorLength = 15f;
    public Vector3 frontSensorPosition;

    public float frontSideSensorPosition;
    public float frontSensorAngle;

    private bool avoiding = false;
    float avoidMultiplier = 0;
    Vector3 sensorStartPos;

    void Update()
    {
        CheckSensor();
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (!avoiding)
            {
                if (destinationDistance >= stopDistance)
                {
                    reachedDestination = false;
                    Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                }
                else
                {
                    reachedDestination = true;
                }
            }
            else
            {
                if (destinationDistance >= stopDistance)
                {
                    reachedDestination = false;
                    Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * avoidMultiplier * movementSpeed * Time.deltaTime);
                }
                else
                {
                    reachedDestination = true;
                }
            }
        }
    }
    
    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }

    public void CheckSensor()
    {
        RaycastHit hit;
        sensorStartPos = mTransform.position;
        sensorStartPos += mTransform.forward * frontSensorPosition.z;
        sensorStartPos += mTransform.up * frontSensorPosition.y;
        avoidMultiplier = 0;
        avoiding = false;

        
        //front center
        if (Physics.Raycast(sensorStartPos, mTransform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
            }
        }
        

        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                avoidMultiplier -= 1f;
                avoiding = true;
            }
        }
        //front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.blue);
                avoidMultiplier -= 0.5f;
                avoiding = true;
            }
        }
        
        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                avoidMultiplier += 1f;
                avoiding = true;
            }
        }
        //front left angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Road") && hit.collider.CompareTag("Car"))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.blue);
                avoidMultiplier += 0.5f;
                avoiding = true;
            }
        }
        
    }
}
