using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event_Trigger : MonoBehaviour
{
    public int triggerDelay = 1;
    public bool disableTriggerObj = true;
    public string triggerByTag = "Player";

    public UnityEvent onStart;
    public UnityEvent onCollision;
    public UnityEvent onTrigger;

    private void Start()
    {
        StartCoroutine(CR_Start());
    }
    IEnumerator CR_Start()
    {
        yield return new WaitForSeconds(triggerDelay);
        onStart.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(triggerByTag)) {

            StartCoroutine(CR_Collision());
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerByTag)) {

            StartCoroutine(CR_Trigger());
        }

    }

    IEnumerator CR_Collision()
    {
        yield return new WaitForSeconds(triggerDelay);
        onCollision.Invoke();
    }

    IEnumerator CR_Trigger()
    {
        yield return new WaitForSeconds(triggerDelay);
        onTrigger.Invoke();
    }
}
