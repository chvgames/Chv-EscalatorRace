using UnityEngine;
using UnityEngine.AI;

public class PlayerHandler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Rigidbody rigid;
    public GameObject killEffect;
    public GameObject bloodObj;

    public bool reachedDestination = false;
    public int inArea = -1;
    public bool finalJump = false;
    public bool jumped = false;

    public bool controleable = true;

    private void Update()
    {
        //if (!controleable)
        //    return;

        //if (finalJump )
        //{
        //    if(HUDListner.run)
        //        transform.Translate(transform.forward * 0.3f);
        //}
        //else {

        //    if (inArea == GameplayScript.currentArea)
        //    {
        //        agent.isStopped = !HUDListner.run;
        //    }

        //    if (!reachedDestination)
        //    {
        //        if (Vector3.Distance(this.transform.position, agent.destination) < 1.5f && !reachedDestination)
        //        {
        //            ReachedDestinationHandling();
        //        }
        //    }
        //}
    }

    public void ReachedDestinationHandling() {

        //Debug.LogError("Reached Destination");
        
        agent.isStopped = true;
        reachedDestination = true;
    }

    public void SetTarget(Vector3 _pos) {

        agent.SetDestination(_pos);
        reachedDestination = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoAhead")){

            inArea++;
            agent.isStopped = false;
        }

        if (other.CompareTag("Jump") && !jumped)
        {
            //Debug.LogError("Jump");
            jumped = true;
            controleable = false;
            //agent.isStopped = true;
            agent.enabled = false;
            rigid.isKinematic = false;
            //this.transform.rotation = other.transform.rotation;
            rigid.mass = 4;
            rigid.AddForce((transform.forward + transform.up) * 30, ForceMode.Impulse);
        }

        if (other.CompareTag("Killer"))
        {
            Debug.LogError("Kill");
            Die();

        }

        if (other.CompareTag("Gate"))
        {
            other.GetComponent<GateHandler>().PerformAction(this.transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("finish")) {

            //agent.enabled = true;
            //agent.isStopped = true;
            //Debug.LogError("Finish");
            this.transform.parent = collision.transform;
        }
    }

    public void Die()
    {

        controleable = false;
        agent.enabled = false;
        rigid.isKinematic = false;

        //Toolbox.GameplayScript.RemovePlayerArmy(this);

        if(killEffect)
            Instantiate(killEffect, this.transform.position, Quaternion.identity);

        if (bloodObj) {

            bloodObj.SetActive(true);
            bloodObj.transform.rotation = Quaternion.Euler(new Vector3(90, 0, Random.Range(0, 180)));
            bloodObj.transform.parent = null;

            Destroy(bloodObj, 5);
        }

        Destroy(this.gameObject);
    }
}
