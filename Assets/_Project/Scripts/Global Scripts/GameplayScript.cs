using System.Collections.Generic;
using UnityEngine;
using System.Collections;
/// <summary>
/// This script will hold everything that is needed to be global only in Game scene
/// </summary>
public class GameplayScript : MonoBehaviour {
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
    private int currentArea = -1;

    public int LevelCompleteTime { get => levelCompleteTime; set => levelCompleteTime = value; }
    public bool DoubleRewardBought { get => doubleRewardBought; set => doubleRewardBought = value; }

    void Awake() {

        //if (!FindObjectOfType<Toolbox>())
        //    Instantiate(Resources.Load("Toolbox"), Vector3.zero, Quaternion.identity);

        Toolbox.Set_GameplayScript(this.GetComponent<GameplayScript>());
    }

    void Start()
    {
        //if (!Toolbox.DB.prefs.GameAudio)
        //    AudioListener.volume = 0.0f;

        //StartCoroutine(GameplayTime());
        levelCompleted = false;
        HUDListner.run = false;

        if (Toolbox.GameManager.lastPlayedLevel != Toolbox.DB.prefs.LastSelectedLevel) {

            Toolbox.GameManager.lastPlayedLevel = Toolbox.DB.prefs.LastSelectedLevel;
            Toolbox.GameManager.curLevelFailed = 0;
        }

        if (!Toolbox.DB.prefs.HasShadows && levelsManager.CurLevelData.weather == LevelData.Weather.SUNNY) {

            foreach (var item in FindObjectsOfType<Light>())
            {
                item.shadows = LightShadows.None;
            }
        }

        AdsManager.instance.RequestAd(AdsManager.AdType.INTERSTITIAL);

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
    }

    public void StartGame()
    {
        Toolbox.HUDListner.EnableHUD();
        Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.gameBG);
    }
    private void EnableEnvHandling()
    {
        environments[Toolbox.DB.prefs.LastSelectedEnv].gameObject.SetActive(true);
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

        levelCompleted = true;

        Toolbox.GameManager.Instantiate_LevelComplete(1);
        Toolbox.HUDListner.DisableHUD();

    }

    public void LevelFailHandling() {

        if (levelFailed || levelCompleted)
            return;

        levelFailed = true;

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

    public void CheckAreaCompletion() {

        if (currentArea+1 >= areaListners.Length)
            return;

        if (areaListners[currentArea+1].inArea >= totalPlayersAvailable) {

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

    public void SetNextAreaCam() {

        cameraListner.target = areaListners[currentArea].camPosition;
        if(cameraListner.target == areaListners[1].camPosition)
        {
            parkingBarrier[0].GetComponent<Animator>().enabled = true;
            parkingBarrier[2].GetComponent<Animator>().enabled = true;
            parkingBarrier[4].GetComponent<Animator>().enabled = true;
        }
        else if (cameraListner.target == areaListners[2].camPosition)
        {
            parkingBarrier[1].GetComponent<Animator>().enabled = true;
            parkingBarrier[3].GetComponent<Animator>().enabled = true;
            parkingBarrier[5].GetComponent<Animator>().enabled = true;
        }

        foreach (var item in activeCharacterArmy)
        {
            item.SetCanRun(true);
        }

        if (levelsManager.isBossLevel) {

            if (currentArea == areaListners.Length - 1) {

                Invoke("EnableLastFightBtn", 1);
                inFight = true;
            }
        }
    }

    public void EnableLastFightBtn() { 
    
        Toolbox.HUDListner.fightBtn.SetActive(true);
    }

    public void AddFinisher() {

        totalPlayersFinished++;

        if (totalPlayersFinished >= totalPlayersAvailable) {

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
