using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHandler : MonoBehaviour
{
    public Transform target;
    public float movementSpeed = 0.1f;
    public float rotationSpeed = 0.1f;

    private void Update()
    {
        if (target) {

            if (Toolbox.GameplayScript.useSlowPowerup)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, target.position, movementSpeed * Time.unscaledDeltaTime);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, target.transform.rotation, rotationSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                this.transform.position = Vector3.Lerp(this.transform.position, target.position, movementSpeed * Time.deltaTime);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, target.transform.rotation, rotationSpeed * Time.deltaTime);
            }
        }

    }
}
