using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class CharacterHandler : MonoBehaviour
{
    public WallObstacle wallObstacle;
    public bool isBossCharacter = false;
    public float speed = 1;
    public Rigidbody rigid;
    public GameObject killEffect;
    public GameObject bloodObj;
    public Transform target;
    public GameObject basicCharacterMesh;

    public GameObject[] characterModels;
    public GameObject[] gunModels;

    public bool autoMove = false;
    public bool reachedDestination = false;
    public bool canRun = false;
    public bool jumped = false;
    public bool finished = false;
    public bool fight = false;

    public Vector3 startRotation;

    bool areaTriggered = false;
    public bool isDead = false;
    public bool wall = false;
    //private void Update()
    //{
    //    if (!controleable)
    //        return;

    //    //if (finalJump)
    //    //{
    //    if (HUDListner.run)
    //        transform.Translate(transform.forward * 0.3f);
    //    //}

    //    AutoMove();
    //}

    private void Start()
    {
        startRotation = this.transform.rotation.eulerAngles;

        if (!isBossCharacter)
            EnableCharacterThings();
    }

    private void FixedUpdate()
    {
        if (fight)
        {
            FightReady();
        }
        else {

            if (jumped)
            {
                JumpMovement();
            }
            else
            {

                if (autoMove)
                {
                    AutoMove();
                }
                else
                {
                    this.transform.rotation = Quaternion.Euler(startRotation);



                    if (HUDListner.run && canRun)
                    {
                        SimpletMove();
                    }
                }
            }
        }

        
    }

    public void EnableCharacterThings() {

        List<GameObject> cObj = new List<GameObject>();

        for (int i = 0; i < characterModels.Length; i++)
        {
            if (Toolbox.DB.prefs.CharactersUnlocked[i] == true) {

                cObj.Add(characterModels[i]);
            }
        } 

        if (cObj.Count > 0) {

            basicCharacterMesh.SetActive(false);
        }

        if (cObj.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, cObj.Count);
            
            cObj[rand].SetActive(true);

            for (int i = 0; i < cObj.Count; i++)
            {
                if (i == rand)
                {
                    cObj[i].SetActive(true);
                }
                else
                {
                    cObj[i].SetActive(false);
                }
            }
        }



        List<GameObject> gObj = new List<GameObject>();

        for (int i = 0; i < gunModels.Length; i++)
        {
            if (Toolbox.DB.prefs.SkinsUnlocked[i] == true)
            { 
                gObj.Add(gunModels[i]);
                
            }
        }

        if (gObj.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, gObj.Count);
            gObj[rand].SetActive(true);
            for (int i = 0; i < gObj.Count; i++)
            {
                if (i == rand)
                {
                    gObj[i].SetActive(true);
                }
                else
                {
                    gObj[i].SetActive(false);

                }
            }
        }

    }

    public void SetAutoMove(Transform _target) {

        SetTarget(_target);

        autoMove = true;
    }

    public void SetTarget(Transform _point) {

        target = _point;
    }

    public void SetCanRun(bool _run)
    {
        canRun = _run;
    }

    public void AutoMove() {

        if (wall)
        {
            return;
        }

        if (target) {
                
            if (finished)
            {
                //Debug.LogError("Lerping");
                if (Toolbox.GameplayScript.useSpeedPowerup)
                    this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position, 0.5f);
                else
                    this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position, 0.01f);
            }
            else {
                if (Toolbox.GameplayScript.useSpeedPowerup)
                    transform.Translate(transform.forward * speed * 2f);
                else
                    transform.Translate(transform.forward * speed);
            }

            this.transform.LookAt(target);

            if (Vector3.Distance(this.transform.position, target.transform.position) < 1) {

                autoMove = false;
                //this.transform.position = target.transform.position;
                this.transform.rotation = target.transform.rotation;

                if (finished)
                    this.enabled = false;
            }
        }
    }

    public void SimpletMove()
    {
        if (Toolbox.GameplayScript.useSpeedPowerup)
            transform.Translate(transform.forward * speed * 2f);
        else
            transform.Translate(transform.forward * speed);
    }

    public void JumpMovement()
    {
        if (Toolbox.GameplayScript.useSpeedPowerup)
            this.transform.position = Vector3.Slerp(this.transform.position, target.transform.position, 0.5f);
        else
            this.transform.position = Vector3.Slerp(this.transform.position, target.transform.position, 0.05f);
    }
    public void FightReady()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target.position, 0.1f);
        //this.transform.rotation = target.transform.rotation;

        //transform.Translate(transform.forward * speed);
        this.transform.LookAt(target);
        HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Area") && !areaTriggered)
        {
            //Debug.LogError("IN AREA");
            other.GetComponent<AreaListner>().AddInArea(this);
            areaTriggered = true;
        }

        if (other.CompareTag("GoAhead"))
        {
            //Debug.LogError("IN GO!");
            //Debug.Log(other.transform.parent.name);
            SetAutoMove(other.transform.parent.transform);
            areaTriggered = false;
            SetCanRun(false);
        }

        if (other.CompareTag("Jump") && !jumped)
        {
            //Debug.LogError("Jumped");
            target = Toolbox.GameplayScript.finishPoint;
            SetCanRun(false);
            jumped = true;

            //rigid.AddForce((transform.forward + transform.up) * 100, ForceMode.Impulse);
        }
        if (other.CompareTag("Enemy") && !fight)
        {

            jumped = false;
            target = Toolbox.GameplayScript.fightPoint;
           // other.GetComponent<AreaListner>().AddInArea(this);
           
            fight = true;
        }

        if (other.CompareTag("finish") && !finished)
        {
            HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
            //Debug.LogError("Alpha!");
            finished = true;
            jumped = false;
            Toolbox.GameplayScript.AddFinisher();
            this.rigid.isKinematic = true;
            other.GetComponent<FinishLister>().AddInArea(this);

            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.checkpoint);
        }

        if (other.CompareTag("Gate"))
        {
            other.GetComponent<GateHandler>().PerformAction(this.transform);
            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        }


        if (other.CompareTag("Killer") && !isDead && !Toolbox.GameplayScript.useShieldPowerup)
        {

            //Debug.LogError("Kill");
            Toolbox.GameplayScript.totalDeaths++;
            Die();
            HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
            Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.vehicleUpgrade);
        }


        if (other.CompareTag("Wall"))
        {
            wall = true;
            wallObstacle.AddPrisoners(gameObject.transform);
        }

        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
            wall = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isBossCharacter && !isDead)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                CharacterHandler characterHandler = collision.collider.GetComponentInParent<CharacterHandler>();

                if (!characterHandler.isDead) {

                    characterHandler.Die();

                    if (Toolbox.GameplayScript.totalBossPlayersAvailable > 0 && !Toolbox.GameplayScript.useShieldPowerup)
                    {
                        Die();
                    }

                    if (Toolbox.GameplayScript.totalBossPlayersAvailable > 0 && Toolbox.GameplayScript.totalPlayersAvailable <= 0)
                    {

                        Toolbox.GameplayScript.LevelFailHandling();
                        Toolbox.GameplayScript.StopFight();
                    }
                    else if (Toolbox.GameplayScript.totalBossPlayersAvailable <= 0 && Toolbox.GameplayScript.totalPlayersAvailable > 0)
                    {
                        Toolbox.GameplayScript.LevelCompleteHandling();
                        Toolbox.GameplayScript.StopFight();
                    }
                }
                
            }
        }

    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (isBossCharacter)
        {
            Toolbox.GameplayScript.RemoveBossArmy(this);
        }
        else {
            Toolbox.GameplayScript.RemovePlayerArmy(this);
        }
        rigid.isKinematic = false;
        isDead = true;

        if (killEffect)
            Instantiate(killEffect, this.transform.position, Quaternion.identity);

        if (bloodObj)
        {
            bloodObj.SetActive(true);
            bloodObj.transform.rotation = Quaternion.Euler(new Vector3(90, 0, UnityEngine.Random.Range(0, 180)));
            bloodObj.transform.parent = null;

            Destroy(bloodObj, 5);
        }

        Destroy(this.gameObject);
    }
}
