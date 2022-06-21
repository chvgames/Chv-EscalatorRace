using UnityEngine;

public class VehicleHandler : MonoBehaviour
{
    private float speed = 1;
    public bool hasFixedSpeed = false;

    public float minSpeedRange = 1;
    public float maxSpeedRange = 1;


    public Transform initPoint;
    public Transform endPoint;

    private void Start()
    {
        if (hasFixedSpeed)
        {
            speed = maxSpeedRange;
        }
        else { 
        
            speed = Random.Range(minSpeedRange, maxSpeedRange);
        }

    }

    private void FixedUpdate()
    {
        this.transform.position += (this.transform.forward) * Time.deltaTime * speed;

        if (Vector3.Distance(this.transform.position, endPoint.transform.position) < 2) {

            this.transform.position = initPoint.position;

            if (this.GetComponent<FinishLister>()) {

                this.GetComponent<FinishLister>().DisableAllCharacters();
            }
        }
    }
}
