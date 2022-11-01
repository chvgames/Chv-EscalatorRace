using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRB;
    public float speed;

    //  ==========================================

    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public WallObstacle wallObstacle;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<CharacterHandler>().isDead)
                return;

            collision.gameObject.GetComponent<CharacterHandler>().Die();
            HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
            wallObstacle.playersTF.Remove(collision.gameObject.transform);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        DestroyMeBullet();
    }

    private void FixedUpdate()
    {
        MoveToEnemy();
    }

    private void MoveToEnemy()
    {
        if (currentTarget == null)
            return;

        transform.LookAt(new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y + 1f, currentTarget.transform.position.z));
        bulletRB.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    private void DestroyMeBullet()
    {
        if (currentTarget == null || !wallObstacle.canShoot)
            Destroy(gameObject);
    }
}
