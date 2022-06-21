using UnityEngine;

public class RoadHandler : MonoBehaviour
{
    private GameObject vehicle;

    public GameObject[] rangeOfVehicles;


    public int totalVehicles = 1;
    public float spawnDelay = 2;
    float time = 0;
    int carsSpawned = 0;
    public bool isFinishRoad = false;

    public Transform initPoint;
    public Transform endPoint;

    private void Start()
    {
        if(rangeOfVehicles.Length > 0)
            vehicle = rangeOfVehicles[Random.Range(0, rangeOfVehicles.Length - 1)];
        else
            vehicle = rangeOfVehicles[0];

        time = 0;
    }

    private void Update()
    {
        if (carsSpawned >= totalVehicles)
            return;


        time -= Time.deltaTime;

        if (time <= 0) {

            time = spawnDelay;
            carsSpawned++;
            SpawnVehicle();
        }
    }

    public void SpawnVehicle() {

        GameObject obj = Instantiate(vehicle, initPoint.position, initPoint.rotation, this.transform);
        VehicleHandler handl = obj.GetComponent<VehicleHandler>();
        
        obj.SetActive(true);

        handl.initPoint = initPoint;
        handl.endPoint = endPoint;
    }
}
