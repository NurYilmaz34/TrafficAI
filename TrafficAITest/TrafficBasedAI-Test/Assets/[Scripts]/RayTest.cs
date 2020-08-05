using UnityEngine;

public class RayTest : MonoBehaviour
{
    [Header("Sensors")]
    public float sensorLength = 5f;
    //public Vector3 frontSensorPosition;
    public Transform mtransform;

    private void FixedUpdate()
    {
        CheckSensor();
    }
    public void CheckSensor()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = mtransform.position;

        if (Physics.Raycast(sensorStartPos, mtransform.forward, out hit, sensorLength))
        {

        }
        Debug.DrawLine(sensorStartPos, hit.point);
    }

}
