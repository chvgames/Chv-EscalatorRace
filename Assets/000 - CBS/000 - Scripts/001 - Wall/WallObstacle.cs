using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WallObstacle : MonoBehaviour
{
    private event EventHandler prisonerCountChange;
    public event EventHandler onPrisonerCountChange
    {
        add
        {
            if (prisonerCountChange == null || !prisonerCountChange.GetInvocationList().Contains(value))
                prisonerCountChange += value;
        }
        remove { prisonerCountChange -= value; }
    }
    private int prisonersCount;
    public int PrisonerCount
    {
        get => prisonersCount;
        set
        {
            prisonersCount = value;
            prisonerCountChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  ========================================================

    [SerializeField] private GameplayScript gameplayScript;
    [SerializeField] private TextMeshProUGUI prisonerCountText;
    [SerializeField] private float xDisable;
    [SerializeField] private int minRequired;
    [SerializeField] private int maxRequired;

    [Space]
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject smoke;
    [SerializeField] private GameObject trigger;

    //  ========================================================

    private int currentCount;
    private int selectedCount;
    [HideInInspector] public bool canShoot;
    [HideInInspector] public List<Transform> playersTF;

    //  ========================================================

    private void OnEnable()
    {
        canShoot = true;
        playersTF = new List<Transform>();
        trigger.transform.position = new Vector3(0f, trigger.transform.position.y, transform.transform.position.z);
        currentCount = UnityEngine.Random.Range(minRequired, maxRequired);
        selectedCount = currentCount;
        prisonerCountText.text = currentCount.ToString();
        prisonersCount = 0;
        onPrisonerCountChange += Prisoners;
    }

    private void OnDisable()
    {
        onPrisonerCountChange -= Prisoners;
        playersTF.Clear();
    }

    private void Prisoners(object sender, EventArgs e)
    {
        if (currentCount <= 0)
            StartCoroutine(PassThrough());

        if (PrisonerCount >= gameplayScript.totalPlayersAvailable)
        {
            if (currentCount > 0)
            {
                gameplayScript.LevelFailHandling();
            }
        }
    }

    IEnumerator PassThrough()
    {
        canShoot = false;

        yield return new WaitForSeconds(1f);

        wall.SetActive(false);
        smoke.SetActive(true);
        trigger.transform.position = new Vector3(xDisable, trigger.transform.position.y, transform.transform.position.z);

        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }

    public void AddPrisoners(Transform gameObj)
    {
        if (currentCount <= 0)
        {
            currentCount = 0;
            return;
        }

        if (!playersTF.Contains(gameObj))
        {
            currentCount -= 1;
            playersTF.Add(gameObj);
        }

        PrisonerCount += 1;
        prisonerCountText.text = currentCount.ToString();
    }

    public void RemovePrisoners()
    {
        if (currentCount >= selectedCount)
        {
            currentCount = selectedCount;
            return;
        }
        currentCount += 1;
        PrisonerCount -= 1;
        prisonerCountText.text = currentCount.ToString();
    }
}
