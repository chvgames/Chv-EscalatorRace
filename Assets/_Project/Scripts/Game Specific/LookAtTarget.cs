using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;
    public GameObject arrowObj;


    private void Update()
    {
        if (target)
            this.transform.LookAt(target);
    }

    public void SetTarget(Transform _val) {

        target = _val;
    }

    public void Status(bool _val) {

        this.arrowObj.SetActive(_val);
    }
}
