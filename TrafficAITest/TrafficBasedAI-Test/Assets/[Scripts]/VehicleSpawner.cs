using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject vehiclePrefab;

    public Transform vehicleContains;

    public int vehiclesToSpawn;

    public Coroutine cor;

    void Start()
    {
        cor = StartCoroutine(SpawnCar());
    }

    IEnumerator SpawnCar()
    {
        if (cor == null)
        {
            int count = 0;
            while (count < vehiclesToSpawn)
            {
                GameObject newVehicle = Instantiate(vehiclePrefab, vehicleContains);
                Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
                newVehicle.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
                newVehicle.transform.position = child.position;
                yield return new WaitForSeconds(1.5f);
                count++;
            }
            cor = null;
        }
    }
}
