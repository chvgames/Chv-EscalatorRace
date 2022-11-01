using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// This script will hold everything that is needed to be global only in Game scene
/// </summary>
public class GameplayScript : MonoBehaviour {

    [Header("TUTORIAL")]
    public bool onTutorial;
    public bool onMainMenu;
    public Button playButton;
    public GameObject hud;

    [Header("NEW GAMEPLAY")]
    public StageData stageData;
    public Transform startPos;

    [Header("NEW GAMEPLAY CITY STAGE")]
    public GameObject townStart;
    public GameObject townIntersection;
    public GameObject townOne;
    public GameObject townTwo;
    public GameObject townNearEnd;
    public GameObject townEnd;
    public GameObject townBossFight;

    [Header("NEW GAMEPLAY SNOWY CITY STAGE")]
    public GameObject snowyTownStart;
    public GameObject snowyTownIntersection;
    public GameObject snowyTownOne;
    public GameObject snowyTownTwo;
    public GameObject snowyTownNearEnd;
    public GameObject snowyTownEnd;
    public GameObject snowyTownBossFight;

    [Header("NEW GAMEPLAY DESERT CITY STAGE")]
    public GameObject desertTownStart;
    public GameObject desertTownIntersectionOne;
    public GameObject desertTownIntersectionTwo;
    public GameObject desertTownOne;
    public GameObject desertTownTwo;
    public GameObject desertTownNearEnd;
    public GameObject desertTownEnd;
    public GameObject desertTownBossFight;

    [Header("NEW GAMEPLAY POWERUPS TIMER")]
    public TextMeshProUGUI divideTimer;
    public TextMeshProUGUI slowTimer;
    public TextMeshProUGUI speedTimer;
    public TextMeshProUGUI shieldTimer;
    public TextMeshProUGUI gameTimer;
    public Animator gameTimerAnimator;
    public Animator divideAnimator;
    public Animator slowAnimator;
    public Animator speedAnimator;
    public Animator shieldAnimator;

    [Header("REAL GAME")]
    public bool testMode = true;
    public GameObject[] parkingBarrier;
    public bool isLevelsScene = true;

    public bool levelCompleted = false;
    public bool levelFailed = false;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform areaSpawnPoint;
    public CamHandler cameraListner;

    public GameObject [] environments;
    public GameObject [] environmentFinalRamp;
    public GameObject [] environmentBossCastle;
    public GameObject [] environmentExtras;

    [Space]
    public List<GameObject> wallsFirstLevel;
    public List<GameObject> wallsSecondLevel;
    public List<GameObject> wallsThirdLevel;

    [HideInInspector]
    private int levelCompleteTime = 0;

    private bool doubleRewardBought = false;
    
    [HideInInspector]
    public bool inFight = false;

    [Header("Components")]
    public AudioListener camListner;
    public bool canShowReviewMenu = false;
    public LevelsManager levelsManager;

    [Header("Colors")]
    public Color[] randomColors;

    [Space(20)]
    [Header("Env Stuff")]
    public Transform startSpawnPoint;
    public Transform bossSpawnPoint;
    public Transform finishPoint;
    public Transform fightPoint;
    //public AreaHandler[] areas;
    public AreaListner bossFightArea;
    public GameObject jumpTrigger;
    public GameObject finalRoad;
    public AreaListner[] areaListners;
    public BossArmyHandler bossArea;
    //public int curPlayersReachedDestination = 0;
    //public int curPlayersReachedNextArea = 0;
    public int totalPlayersFinished = 0;
    public int totalPlayersAvailable = 0;
    public int totalBossPlayersAvailable = 0;
    public List<CharacterHandler> activeCharacterArmy;
    public List<CharacterHandler> activeBossArmy;

    //screenshot requirement
    int screenShotPicName = 0;
    public int currentArea = 0;
    int currentCounter = 0;
    int lastCounter = 0;

    public int LevelCompleteTime { get => levelCompleteTime; set => levelCompleteTime = value; }
    public bool DoubleRewardBought { get => doubleRewardBought; set => doubleRewardBought = value; }

    //  ===================================

    #region TUTORIAL VARIABLES

    [HideInInspector] public bool doneTapMove;
    [HideInInspector] public bool doneHoldMove;

    #endregion

    #region OPTIMIZATION

    private List<Transform> gameplayObjects;
    [HideInInspector] public List<AreaListner> areaListenersList;
    [HideInInspector] public List<EnvironmentArea> environmentAreas;
    [HideInInspector] public bool doneInitialization;

    public int totalDeaths;

    #endregion

    #region POWERUPS VARIABLE

    bool doneIntializeGameTimer;
    bool isLessThanTenSeconds;
    [HideInInspector] public bool useDividePowerup;
    [HideInInspector] public bool useSlowPowerup;
    [HideInInspector] public bool useSpeedPowerup;
    [HideInInspector] public bool useShieldPowerup;

    float currentDividePowerupTime;
    float currentSlowPowerupTime;
    float currentSpeedPowerupTime;
    float currentShieldPowerupTime;

    float currentTimer;

    #endregion

    //  ===================================

    void Awake()
    {
        if (!onMainMenu && !onTutorial)
        {
            doneInitialization = false;
            gameplayObjects = new List<Transform>();
            areaListenersList = new List<AreaListner>();
            StartCoroutine(InitializeStages());
        }

        //if (!FindObjectOfType<Toolbox>())
        //    Instantiate(Resources.Load("Toolbox"), Vector3.zero, Quaternion.identity);

        //if (Toolbox.DB.prefs.CurrentTimer <= 0 && !onTutorial) Toolbox.DB.prefs.CurrentTimer = 30;

        //if (Toolbox.DB.prefs.FireSpeed <= 0 && !onTutorial) Toolbox.DB.prefs.FireSpeed = 5f;

        Toolbox.Set_GameplayScript(this.GetComponent<GameplayScript>());
    }

    void Start()
    {
        if (!Toolbox.DB.prefs.GameAudio)
            AudioListener.volume = 0.0f;

        //if (Toolbox.GameManager.lastPlayedLevel != Toolbox.DB.prefs.LastSelectedLevel && !onTutorial &&
        //    !onMainMenu) {

        //    Toolbox.GameManager.lastPlayedLevel = Toolbox.DB.prefs.LastSelectedLevel;
        //    Toolbox.GameManager.curLevelFailed = 0;
        //}

        if (!Toolbox.DB.prefs.HasShadows && levelsManager.CurLevelData.weather == LevelData.Weather.SUNNY)
        {

            foreach (var item in FindObjectsOfType<Light>())
            {
                item.shadows = LightShadows.None;
            }
        }

        if (!onTutorial)
            AdsManager.instance.RequestAd(AdsManager.AdType.INTERSTITIAL);
        
        if (!onMainMenu)
            playButton.onClick.Invoke();

        if (onMainMenu)
            SetNextAreaCam();

        EnableEnvHandling();
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            string name = "Pic_" + screenShotPicName + ".png";
            Toolbox.GameManager.Log("Screenshot Taked!");
            ScreenCapture.CaptureScreenshot(name);
            screenShotPicName++;
        }
#endif
        GameTimer();
        DivideTimer();
        SlowTimer();
        SpeedTimer();
        ShieldTimer();
    }

    #region TUTORIAL


    #endregion

    
    #region NEW GAMEPLAY

    IEnumerator InitializeStages()
    {
        switch (stageData.CurrentStage)
        {
            case StageData.Stage.CITY:

                yield return StartCoroutine(StageInstantiate(townStart, townIntersection, townOne,
                    townTwo, townNearEnd, townEnd, townBossFight, false));

                break;
            case StageData.Stage.DESERT:

                yield return StartCoroutine(StageInstantiate(desertTownStart, null, desertTownOne,
                    desertTownTwo, desertTownNearEnd, desertTownEnd, desertTownBossFight, true));

                break;
            case StageData.Stage.SNOW:

                yield return StartCoroutine(StageInstantiate(snowyTownStart, snowyTownIntersection, snowyTownOne,
                    snowyTownTwo, snowyTownNearEnd, snowyTownEnd, snowyTownBossFight, false));

                break;
        }
    }

    IEnumerator StageInstantiate(GameObject startStage, GameObject intersectionStage, GameObject stageOne,
        GameObject stageTwo, GameObject nearEndStage, GameObject endStage, GameObject bossStage, bool onDesert)
    {
        float lastVehicleSpeed = 5;
        int index = 0;

        #region INSTANTIATE TOWN

        //  INSTANTIATE START
        GameObject start = Instantiate(startStage);
        start.transform.position = startPos.position;
        gameplayObjects.Add(start.transform);
        areaListenersList.Add(start.GetComponent<EnvironmentArea>().areaListner);
        environmentAreas.Add(start.GetComponent<EnvironmentArea>());

        //  INSTANTIATE FIRST INTERSECTION
        GameObject intersection;
        if (onDesert)
        {
            int desertIntersection = Random.Range(0, 2);
            if (desertIntersection == 0)
            {
                intersection = Instantiate(desertTownIntersectionOne);
                intersection.transform.position = start.GetComponent<EnvironmentArea>().spawnPoint.position;
                intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[0].vehicleSpeed = lastVehicleSpeed;
            }
            else
            {
                intersection = Instantiate(desertTownIntersectionTwo);
                intersection.transform.position = start.GetComponent<EnvironmentArea>().spawnPointTwo.position;
                intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[0].vehicleSpeed = lastVehicleSpeed;
            }
        }
        else
        {
            intersection = Instantiate(intersectionStage);

            intersection.transform.position = start.GetComponent<EnvironmentArea>().spawnPoint.position;
            intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[0].vehicleSpeed = lastVehicleSpeed;
            intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[1].vehicleSpeed = lastVehicleSpeed;
        }

        gameplayObjects.Add(intersection.transform);    //  SETTING UP AREA LIST

        lastVehicleSpeed += 5;
        Debug.Log(stageData.currentStageNumber);

        //  INSTANTIATING TOWN OBJECTS
        for (int a = 0; a < stageData.currentStageNumber; a++)
        {
            GameObject town = null;

            switch (index)
            {
                case 0:
                    town = Instantiate(stageOne);
                    town.transform.position = intersection.GetComponent<EnvironmentTownIntersection>().town1SpawnPosition.position;
                    gameplayObjects.Add(town.transform);
                    areaListenersList.Add(town.GetComponent<EnvironmentArea>().areaListner);
                    environmentAreas.Add(town.GetComponent<EnvironmentArea>());
                    break;
                case 1:
                    town = Instantiate(stageTwo);
                    town.transform.position = intersection.GetComponent<EnvironmentTownIntersection>().town2SpawnPosition.position;
                    gameplayObjects.Add(town.transform);
                    areaListenersList.Add(town.GetComponent<EnvironmentArea>().areaListner);
                    environmentAreas.Add(town.GetComponent<EnvironmentArea>());
                    break;
            }

            if (onDesert)
            {
                int desertIntersection = Random.Range(0, 2);
                if (desertIntersection == 0)
                {
                    intersection = Instantiate(desertTownIntersectionOne);
                    intersection.transform.position = town.GetComponent<EnvironmentArea>().spawnPoint.position;
                    intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[0].vehicleSpeed = lastVehicleSpeed;
                }
                else
                {
                    intersection = Instantiate(desertTownIntersectionTwo);
                    intersection.transform.position = town.GetComponent<EnvironmentArea>().spawnPointTwo.position;
                    intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[0].vehicleSpeed = lastVehicleSpeed;
                }
            }
            else
            {
                intersection = Instantiate(intersectionStage);

                intersection.transform.position = town.GetComponent<EnvironmentArea>().spawnPoint.position;
                intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[0].vehicleSpeed = lastVehicleSpeed;
                intersection.GetComponent<EnvironmentTownIntersection>().roadHandlers[1].vehicleSpeed = lastVehicleSpeed;
            }

            gameplayObjects.Add(intersection.transform);

            if (lastVehicleSpeed < 60)
                lastVehicleSpeed += 5;

            if (a > 2)
            {
                if (currentCounter == 0) currentCounter = 6;

                town.SetActive(false);
                intersection.SetActive(false);
            }

            index++;

            if (index > 1)
                index = 0;

            yield return null;
        }

        //  INSTANTIATE 
        GameObject nearEnd = Instantiate(nearEndStage);
        nearEnd.transform.position = intersection.GetComponent<EnvironmentTownIntersection>().townNearEnd.position;
        gameplayObjects.Add(nearEnd.transform);

        if (currentCounter != 0)
            nearEnd.SetActive(false);


        if (stageData.isBossFight)
        {
            GameObject end = Instantiate(bossStage);
            end.transform.position = nearEnd.GetComponent<EnvironmentNearEnd>().townBoss.position;
            gameplayObjects.Add(end.transform);
            areaListenersList.Add(end.GetComponent<EnvironmentArea>().areaListner);
            environmentAreas.Add(end.GetComponent<EnvironmentArea>());
            bossSpawnPoint = end.GetComponent<EnvironmentArea>().bossSpawnPoint;
            fightPoint = end.GetComponent<EnvironmentArea>().fightPoint;
            bossArea = end.GetComponent<EnvironmentArea>().bossArmyHandler;
            bossFightArea = end.GetComponent<EnvironmentArea>().areaListner;

            if (currentCounter != 0)
                end.SetActive(false);
        }
        else
        {
            GameObject end = Instantiate(endStage);
            end.transform.position = nearEnd.GetComponent<EnvironmentNearEnd>().townEnd.position;

            gameplayObjects.Add(end.transform);
            areaListenersList.Add(end.GetComponent<EnvironmentArea>().areaListner);
            environmentAreas.Add(end.GetComponent<EnvironmentArea>());
            finishPoint = end.GetComponent<EnvironmentArea>().finishPoint;
            finalRoad = end.GetComponent<EnvironmentArea>().finalRoad;
            jumpTrigger = end.GetComponent<EnvironmentArea>().jumpTrigger;
            end.GetComponent<EnvironmentArea>().roadHandlerEnd.vehicleSpeed = 20;

            if (currentCounter != 0)
                end.SetActive(false);
        }

        #endregion

        doneInitialization = true;
        levelCompleted = false;
        HUDListner.run = false;
    }

    private void OptimizationStages()
    {
        if (currentCounter == 0 || currentArea == 0 || currentCounter + 1 >= gameplayObjects.Count - 1)
            return;

        currentCounter += 2;

        if (currentCounter > gameplayObjects.Count)
            return;

        gameplayObjects[currentCounter].gameObject.SetActive(true);
        gameplayObjects[currentCounter + 1].gameObject.SetActive(true);
        gameplayObjects[lastCounter].gameObject.SetActive(false);
        gameplayObjects[lastCounter + 1].gameObject.SetActive(false);
        lastCounter += 2;
    }

    public void CheckAreaCompletion()
    {
        if (onTutorial)
        {
            if (currentArea + 1 >= areaListners.Length)
                return;

            else if (areaListners[currentArea + 1].inArea >= totalPlayersAvailable)
            {
                if (currentArea + 1 == 0)
                {
                    playButton.onClick.Invoke();
                    hud.SetActive(true);
                }

                currentArea++;


                if (currentArea < areaListners.Length)
                {
                    //Debug.LogError("Area Complete. Next = " + currentArea);

                    Invoke("SetNextAreaCam", 1);
                }
                //if(currentArea == 4)
                //{
                //    bossFightArea.SetActive(true);
                //}
            }
        }
        else
        {
            if (currentArea + 1 >= areaListenersList.Count)
                return;

            if (areaListenersList[currentArea + 1].inArea >= totalPlayersAvailable)
            {

                currentArea++;

                if (currentArea < areaListenersList.Count)
                {
                    //Debug.LogError("Area Complete. Next = " + currentArea);

                    Invoke("SetNextAreaCam", 1);
                }
                //if(currentArea == 4)
                //{
                //    bossFightArea.SetActive(true);
                //}
            }
        }
    }

    public void SetNextAreaCam()
    {
        if (onTutorial)
        {
            cameraListner.target = areaListners[currentArea].camPosition;
            if (cameraListner.target == areaListners[1].camPosition)
            {
                Toolbox.GameManager.Instantiate_FirstTutorial();
                parkingBarrier[0].GetComponent<Animator>().enabled = true;
                parkingBarrier[2].GetComponent<Animator>().enabled = true;
                parkingBarrier[4].GetComponent<Animator>().enabled = true;
            }
            else if (cameraListner.target == areaListners[2].camPosition)
            {
                Toolbox.GameManager.Instantiate_SecondTutorial();
                parkingBarrier[1].GetComponent<Animator>().enabled = true;
                parkingBarrier[3].GetComponent<Animator>().enabled = true;
                parkingBarrier[5].GetComponent<Animator>().enabled = true;
            }
            else if (cameraListner.target == areaListners[3].camPosition)
                Toolbox.GameManager.Instantiate_ThirdTutorial();
        }
        else if (onMainMenu)
        {
            cameraListner.target = areaListners[0].camPosition;
        }
        else
        {
            cameraListner.target = areaListenersList[currentArea].camPosition;

            if (environmentAreas[currentArea].gateAnimator != null)
                environmentAreas[currentArea].gateAnimator.enabled = true;

            if (levelsManager.isBossLevel)
            {
                if (currentArea == areaListenersList.Count - 1)
                {

                    Invoke("EnableLastFightBtn", 1);
                    inFight = true;
                }
            }

            OptimizationStages();
        }


        foreach (var item in activeCharacterArmy)
        {
            item.SetCanRun(true);
        }
    }

    public void UnlockNextStage()
    {
        if (stageData.currentStageNumber < 500)
        {
            if (Toolbox.DB.prefs.CurrentLevel < stageData.currentStageNumber)
                Toolbox.DB.prefs.CurrentLevel = stageData.currentStageNumber - 1;

            Toolbox.DB.prefs.SetUnlockedLevel(stageData.currentStageNumber, true);
        }

        if (totalDeaths <= 270 * 0.25 && totalDeaths >= 0)
            Toolbox.DB.prefs.SetStarsLevel(stageData.currentStageNumber - 1, 3);
        else if (totalDeaths < 270 * 0.75 && totalDeaths > 270 * 0.25)
            Toolbox.DB.prefs.SetStarsLevel(stageData.currentStageNumber - 1, 2);
        else if (totalDeaths <= 270 && totalDeaths >= 270 * 0.75)
            Toolbox.DB.prefs.SetStarsLevel(stageData.currentStageNumber - 1, 1);
    }

    public void DivideTimer()
    {
        if (onMainMenu && onTutorial)
            return;

        if (!useDividePowerup || levelFailed || levelCompleted)
            return;

        if (currentDividePowerupTime == 0)
            currentDividePowerupTime = 11f;

        float minutes = Mathf.FloorToInt(currentDividePowerupTime / 60);
        float seconds = Mathf.FloorToInt(currentDividePowerupTime % 60);

        divideTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentDividePowerupTime > 0)
            currentDividePowerupTime -= Time.deltaTime;
        else
        {
            divideAnimator.SetTrigger("donePowerup");
            currentDividePowerupTime = 0;
            useDividePowerup = false;
        }
    }

    public void SlowTimer()
    {
        if (onMainMenu && onTutorial)
            return;

        if (!useSlowPowerup || levelFailed || levelCompleted)
            return;

        if (currentSlowPowerupTime == 0)
            currentSlowPowerupTime = 11f;

        float minutes = Mathf.FloorToInt(currentSlowPowerupTime / 60);
        float seconds = Mathf.FloorToInt(currentSlowPowerupTime % 60);

        slowTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentSlowPowerupTime > 0)
            currentSlowPowerupTime -= Time.deltaTime;
        else
        {
            slowAnimator.SetTrigger("donePowerup");
            currentSlowPowerupTime = 0;
            useSlowPowerup = false;
        }
    }

    public void SpeedTimer()
    {
        if (onMainMenu && onTutorial)
            return;

        if (!useSpeedPowerup || levelFailed || levelCompleted)
            return;

        if (currentSpeedPowerupTime == 0)
            currentSpeedPowerupTime = 11f;

        float minutes = Mathf.FloorToInt(currentSpeedPowerupTime / 60);
        float seconds = Mathf.FloorToInt(currentSpeedPowerupTime % 60);

        speedTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentSpeedPowerupTime > 0)
            currentSpeedPowerupTime -= Time.deltaTime;
        else
        {
            speedAnimator.SetTrigger("donePowerup");
            currentSpeedPowerupTime = 0;
            useSpeedPowerup = false;
        }
    }

    public void ShieldTimer()
    {
        if (onMainMenu && onTutorial)
            return;

        if (!useShieldPowerup || levelFailed || levelCompleted)
            return;

        if (currentShieldPowerupTime == 0)
            currentShieldPowerupTime = 11f;

        float minutes = Mathf.FloorToInt(currentShieldPowerupTime / 60);
        float seconds = Mathf.FloorToInt(currentShieldPowerupTime % 60);

        shieldTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentShieldPowerupTime > 0)
            currentShieldPowerupTime -= Time.deltaTime;
        else
        {
            shieldAnimator.SetTrigger("donePowerup");
            currentShieldPowerupTime = 0;
            useShieldPowerup = false;
        }
    }

    public void GameTimer()
    {
        if (onMainMenu && onTutorial)
            return;

        if (!doneInitialization || levelFailed || levelCompleted)
            return;

        if (!doneIntializeGameTimer)
        {
            currentTimer = Toolbox.DB.prefs.CurrentTimer[stageData.currentStageNumber - 1] + 3;
            doneIntializeGameTimer = true;
        }

        float minutes = Mathf.FloorToInt(currentTimer / 60);
        float seconds = Mathf.FloorToInt(currentTimer % 60);

        gameTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (!isLessThanTenSeconds && currentTimer <= 10)
        {
            gameTimerAnimator.SetTrigger("fast");
            Toolbox.Soundmanager.TickerTimer(true);
            isLessThanTenSeconds = true;
        }

        if (currentTimer <= 0)
        {
            Toolbox.Soundmanager.TickerTimer(false);
            LevelFailHandling();
            return;
        }

        currentTimer -= Time.deltaTime;
    }

    #endregion

    public void StartGame()
    {
        Toolbox.HUDListner.EnableHUD();
        Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.gameBG);
    }
    private void EnableEnvHandling()
    {
        //environments[Toolbox.DB.prefs.LastSelectedEnv].gameObject.SetActive(true);

        //int randomizerFirstWall = Random.Range(0, 101);
        //int randomizerSecondWall = Random.Range(0, 101);

        //if (Toolbox.DB.prefs.LastSelectedEnv == 0)
        //{
        //    if (randomizerFirstWall >= 75)
        //        wallsFirstLevel[0].SetActive(true);

        //    if (randomizerSecondWall >= 75)
        //        wallsFirstLevel[0].SetActive(true);
        //}
        //else if (Toolbox.DB.prefs.LastSelectedEnv == 1)
        //{
        //    if (randomizerFirstWall >= 75)
        //        wallsSecondLevel[0].SetActive(true);

        //    if (randomizerSecondWall >= 75)
        //        wallsSecondLevel[0].SetActive(true);
        //}
        //else if (Toolbox.DB.prefs.LastSelectedEnv == 2)
        //{
        //    if (randomizerFirstWall >= 75)
        //        wallsThirdLevel[0].SetActive(true);

        //    if (randomizerSecondWall >= 75)
        //        wallsThirdLevel[0].SetActive(true);
        //}
    }   

    public void IncrementGoldCoins(int cost)
    {
        Toolbox.DB.prefs.GoldCoins += cost;
        Toolbox.GameManager.Instantiate_RewardAnim();

        if (FindObjectOfType<MainMenuListner>())
        {

            FindObjectOfType<MainMenuListner>().UpdateTxt();
        }
    }
    public void EnableEnvironment(int _val)
    {
        for (int i = 0; i < environments.Length; i++)
        {
            if(i != _val)
                environments[i].SetActive(false);
        }

    }
    public void DeductGoldCoins(int cost) {

        Toolbox.DB.prefs.GoldCoins -= cost;

        if (FindObjectOfType<MainMenuListner>()) {

            FindObjectOfType<MainMenuListner>().UpdateTxt();
        }
    }


    public void InitEffectOnpoint(GameObject effect, Vector3 pos) {

        Instantiate(effect, pos, Quaternion.identity);
    }
    
    public Color Get_RandomColor() {

        return randomColors[Random.Range(0, randomColors.Length - 1)];
    }

    public void LevelCompleteHandling() {

        if (levelFailed || levelCompleted)
            return;

        if (!onTutorial)
        {

            Toolbox.Soundmanager.TickerTimer(false);
            if (Toolbox.DB.prefs.IsBossLevel)
            {
                if (Toolbox.DB.prefs.BossesFightDone < 2)
                {
                    Toolbox.DB.prefs.BossesFightDone += 1;

                    if (Toolbox.DB.prefs.BossesFightDone >= 2)
                        Toolbox.DB.prefs.BossesFightDone = 2;
                }

                if (Toolbox.DB.prefs.GatheredTroops < 300)
                {
                    Toolbox.DB.prefs.GatheredTroops += totalPlayersAvailable;

                    if (Toolbox.DB.prefs.GatheredTroops >= 300)
                        Toolbox.DB.prefs.GatheredTroops = 300;
                }
            }

            if (Toolbox.DB.prefs.GatheredTroops < 300)
            {
                Toolbox.DB.prefs.GatheredTroops += totalPlayersFinished;

                if (Toolbox.DB.prefs.GatheredTroops >= 300)
                    Toolbox.DB.prefs.GatheredTroops = 300;
            }

            if (Toolbox.DB.prefs.LevelsDone < 10)
            {
                Toolbox.DB.prefs.LevelsDone += 1;

                if (Toolbox.DB.prefs.LevelsDone >= 10)
                    Toolbox.DB.prefs.LevelsDone = 10;
            }

            if (Toolbox.DB.prefs.FireSpeed > 1f)
            {
                Toolbox.DB.prefs.FireSpeed -= 0.25f;

                if (Toolbox.DB.prefs.FireSpeed <= 1)
                    Toolbox.DB.prefs.FireSpeed = 1f;
            }

            UnlockNextStage();
            Toolbox.GameManager.Instantiate_LevelComplete(1);
        }
        else
        {
            Toolbox.DB.prefs.DoneTutorial = true;

            Toolbox.GameManager.Instantiate_TutorialComplete(1);
        }

        levelCompleted = true;

        Toolbox.HUDListner.DisableHUD();

    }

    public void LevelFailHandling() {

        if (levelFailed || levelCompleted)
            return;

        levelFailed = true;

        Toolbox.Soundmanager.TickerTimer(false);
        Toolbox.GameManager.Instantiate_LevelFail(1);
        Toolbox.HUDListner.DisableHUD();
    }

    public void SetSkybox(Material _mat) { 
    
        RenderSettings.skybox = _mat;
    }

    public void EnablePlayerVehiclePassengers() {
        
        Toolbox.HUDListner.EnableHUD();
    }


    public GameObject GetPlayerArmyObj() {
        return playerPrefab;
    } 

    public void AddPlayerArmy(CharacterHandler _handler) {

        activeCharacterArmy.Add(_handler);
        totalPlayersAvailable++;
    }

    public void RemovePlayerArmy(CharacterHandler _handler)
    {
        activeCharacterArmy.Remove(_handler);
        totalPlayersAvailable--;

        CheckAreaCompletion();

        if (!inFight) {

            if (totalPlayersFinished >= totalPlayersAvailable)
            {

                if (totalPlayersFinished > 0)
                {
                    LevelCompleteHandling();
                }
                else
                {
                    LevelFailHandling();
                }
            }
        }
    }

    public void AddBossArmy(CharacterHandler _handler)
    {
        activeBossArmy.Add(_handler);
        totalBossPlayersAvailable++;
    }

    public void RemoveBossArmy(CharacterHandler _handler)
    {
        activeBossArmy.Remove(_handler);
        totalBossPlayersAvailable--;
    }

    public void EnableLastFightBtn() {
        Toolbox.HUDListner.fightBtn.SetActive(true);
    }

    public void AddFinisher() {

        totalPlayersFinished++;

        if (totalPlayersFinished >= totalPlayersAvailable)
        {
            LevelCompleteHandling();
        }
    }

    public void StartFight() {

        StartCoroutine(PlayerArmyWar());
        StartCoroutine(BossArmyWar());
    }

    public void StopFight()
    {
        foreach (var item in activeCharacterArmy)
        {
            item.fight = false;
        }

        foreach (var item in activeBossArmy)
        {
            item.fight = false;
        }
    }

    IEnumerator PlayerArmyWar() {

        yield return new WaitForSeconds(0.5f);

        foreach (var item in activeCharacterArmy)
        {
            HUDListner.instance.score.text = totalPlayersAvailable.ToString();
            item.target = fightPoint;
            item.speed = 0.1f;
            item.fight = true;
        }
    }

    IEnumerator BossArmyWar()
    {

        yield return new WaitForSeconds(0.5f);

        foreach (var item in activeBossArmy)
        {
            item.target = fightPoint;
            item.speed = 0.1f;
            item.fight = true;
        }
    }
    public void UpdateCharacterAperaence()
    {
        foreach (var item in activeCharacterArmy)
        {
            item.EnableCharacterThings(); 
        } 
    }
}
