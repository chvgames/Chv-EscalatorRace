using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShooters : MonoBehaviour
{
    public WallObstacle wallObstacle;
    public Transform muzzle;
    public GameObject bulletPrefab;
    public Animator enemyAnimator;
    public float rotationDamp;

    //  =====================================

    private Quaternion rotationAngle;

    //  =====================================

    private float currentTime;

    private void OnEnable()
    {
        currentTime = Toolbox.DB.prefs.FireSpeed;
    }

    private void Update()
    {
        ShootEnemy();
        LookAtEnemy();
    }

    private void LookAtEnemy()
    {
        if (wallObstacle.playersTF.Count <= 0)
            return;

        rotationAngle = Quaternion.LookRotation(wallObstacle.playersTF[0].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * rotationDamp);
    }

    private void ShootEnemy()
    {
        if (wallObstacle.playersTF.Count <= 0 || !wallObstacle.canShoot) return;

        if (currentTime > 0)
            currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            enemyAnimator.SetTrigger("shootthisfucker");
            currentTime = Toolbox.DB.prefs.FireSpeed;
        }
    }

    public void ShootThisFucker()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);

        bullet.GetComponent<Bullet>().currentTarget = wallObstacle.playersTF[0].gameObject;
        bullet.GetComponent<Bullet>().wallObstacle = wallObstacle;
    }
}
