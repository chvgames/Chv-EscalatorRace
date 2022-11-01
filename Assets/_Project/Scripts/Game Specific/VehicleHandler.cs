using UnityEngine;

public class VehicleHandler : MonoBehaviour
{
    public float speed;
    public bool hasFixedSpeed = false;

    public float minSpeedRange = 1;
    public float maxSpeedRange = 1;


    public Transform initPoint;
    public Transform endPoint;

    private void Start()
    {
        if (Toolbox.GameplayScript.onTutorial)
        {
            if (hasFixedSpeed)
            {
                speed = maxSpeedRange;
            }
            else
            {

                speed = Random.Range(minSpeedRange, maxSpeedRange);
            }
        }

    }

    private void FixedUpdate()
    {
        if (Toolbox.GameplayScript.useSlowPowerup)
            this.transform.position += (this.transform.forward) * Time.deltaTime * 0.5f;
        else
            this.transform.position += (this.transform.forward) * Time.deltaTime * speed;

        if (Vector3.Distance(this.transform.position, endPoint.transform.position) < 2) {

            this.transform.position = initPoint.position;

            if (this.GetComponent<FinishLister>()) {

                this.GetComponent<FinishLister>().DisableAllCharacters();
            }
        }
    }
}
