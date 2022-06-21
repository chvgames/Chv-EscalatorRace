using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public bool setTargetAsMainPlayer = true;
    public Vector3 offset;
    public Transform target;
    public float speed = 0.5f;
    private Vector3 targetPos;

    private void OnEnable()
    {


        targetPos = target.position + offset;
        this.transform.position = targetPos;
    }

    public void SetTarget(Transform _obj) {

        target = _obj;
    }
    private void Update()
    {
        if (target) {

            targetPos = target.position + offset;
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, speed * Time.deltaTime);
        }
    }
}
