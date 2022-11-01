using System.Collections;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{

    public StageData stageData;
    public bool testMode = false;
    public bool testPlayerObj = false;
    public int testPlayerObjVal = 0;
    
    [HideInInspector]
    public bool isBossLevel = false;

    private LevelHandler curLevelHandler;
    [SerializeField] private LevelData curLevelData;

    bool spawnArmy = false;
    int curArmySpawned = 0;
    int startPlayerSpawnAmount = 0;
    float spawnDelay = 0.1f;
    float time = 0;
    public LevelData CurLevelData { get => curLevelData; set => curLevelData = value; }
    public LevelHandler CurLevelHandler { get => curLevelHandler; set => curLevelHandler = value; }

    private void Start()
    {
        if (!Toolbox.GameplayScript.onTutorial && !Toolbox.GameplayScript.onMainMenu)
        {
            startPlayerSpawnAmount = Toolbox.DB.prefs.StartSpawnPlayersVal;
            isBossLevel = stageData.isBossFight;
        }
        else
        {
            startPlayerSpawnAmount = 10;
            isBossLevel = false;
            spawnArmy = true;
            time = spawnDelay;
        }

        if (testMode)
            StartCoroutine(StartLevelHandling());
    }

    public IEnumerator StartLevelHandling() {

        while (!Toolbox.GameplayScript.doneInitialization) yield return null;

        if (testMode)
        {
            curLevelHandler = this.GetComponentInChildren<LevelHandler>();
            //Toolbox.DB.prefs.LastSelectedLevel = int.Parse(curLevelHandler.gameObject.name);
        }
        else
        {
            InstantiateLevel();
        }

        LevelDataHandling();
        //PlayerDataHandling();
        SpawnPlayersArmy();

        Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.gameBG);
        Toolbox.GameManager.Analytics_LevelStart();

        if (isBossLevel) {

            //Toolbox.GameplayScript.environmentBossCastle[Toolbox.DB.prefs.LastSelectedEnv].SetActive(true);
            //Toolbox.GameplayScript.environmentFinalRamp[Toolbox.DB.prefs.LastSelectedEnv].SetActive(false);

            Toolbox.GameplayScript.areaListenersList[Toolbox.GameplayScript.areaListenersList.Count - 1] = Toolbox.GameplayScript.bossFightArea;
            Toolbox.GameplayScript.bossFightArea.gameObject.SetActive(true);
        }
    }

    private void InstantiateLevel()
    {
        string path = Constants.PrefabFolderPath + Constants.LevelsFolderPath + Toolbox.DB.prefs.LastSelectedMode.ToString() + "/" + Toolbox.DB.prefs.LastSelectedLevel.ToString();
        //Toolbox.GameManager.Log("Lvl path = " + path);

        GameObject obj = (GameObject)Instantiate(Resources.Load(path), this.transform);
        
        curLevelHandler = obj.GetComponent<LevelHandler>();
    }

    private void SpawnPlayersArmy()
    {
        //for (int i = 0; i < CurLevelData.playerObjInStart; i++)
        //{
        //    GameObject obj = Instantiate(Toolbox.GameplayScript.GetPlayerArmyObj(), Toolbox.GameplayScript.startSpawnPoint.position, Toolbox.GameplayScript.startSpawnPoint.rotation);
        //    Toolbox.GameplayScript.AddPlayerArmy(obj.GetComponent<CharacterHandler>());
        //    obj.SetActive(true);
        //}

        ////Assign Army First Target Position

        //for (int i = 0; i < Toolbox.GameplayScript.activeCharacterArmy.Count; i++)
        //{
        //    Toolbox.GameplayScript.activeCharacterArmy[i].SetAutoMove(Toolbox.GameplayScript.areaListners[0].transform);
        //}

        spawnArmy = true;

        Toolbox.GameplayScript.cameraListner.target = Toolbox.GameplayScript.areaListenersList[0].camPosition;

        if (isBossLevel)
            Toolbox.GameplayScript.bossArea.SpawnBossArmy(startPlayerSpawnAmount);
    }

    private void LevelDataHandling()
    {
        string path;

        if (testMode)
        {
             path = Constants.PrefabFolderPath + Constants.LevelsScriptablesFolderPath + Toolbox.DB.prefs.LastSelectedMode.ToString() + "/" + int.Parse(this.GetComponentInChildren<LevelHandler>().name).ToString() ;
        }
        else
        { 
             path = Constants.PrefabFolderPath + Constants.LevelsScriptablesFolderPath + Toolbox.DB.prefs.LastSelectedMode.ToString() + "/" + Toolbox.DB.prefs.LastSelectedLevel.ToString();
        }
        
        curLevelData = (LevelData)Resources.Load(path);

       // Toolbox.HUDListner.SetLvlTxt("Level " + (Toolbox.DB.prefs.LastSelectedLevel + 1).ToString());

        spawnDelay = CurLevelData.playerObjInStart / 1000;
    }


    public void PlayTutorial() 
    {
        //Toolbox.GameplayScript.GameplayTutorial.SetActive(true);
    }


    private void ExtraHandling()
    {
        if (curLevelData.envProfile)
        {
            for (int i = 0; i < curLevelData.envProfile.materialChange.Length; i++)
            {
                curLevelData.envProfile.materialChange[i].mat.SetTexture(curLevelData.envProfile.materialChange[i].fieldName, curLevelData.envProfile.materialChange[i].albedoTex);

                if (curLevelData.weather == LevelData.Weather.RAINY)
                {
                    //Toolbox.GameManager.Log("isRaining!");
                    curLevelData.envProfile.materialChange[i].mat.SetFloat("_Cutoff", curLevelData.envProfile.materialChange[i].alphaCutout);
                    curLevelData.envProfile.materialChange[i].mat.SetFloat("_Metallic", curLevelData.envProfile.materialChange[i].mettalic);
                    curLevelData.envProfile.materialChange[i].mat.SetFloat("_SmoothnessTextureChannel", curLevelData.envProfile.materialChange[i].smoothness);
                }
                else {
                    curLevelData.envProfile.materialChange[i].mat.SetFloat("_Cutoff", 0);
                    curLevelData.envProfile.materialChange[i].mat.SetFloat("_Metallic", 0);
                    curLevelData.envProfile.materialChange[i].mat.SetFloat("_SmoothnessTextureChannel", 0);
                }
            }

            if (curLevelData.envProfile.skybox)
                Toolbox.GameplayScript.SetSkybox(curLevelData.envProfile.skybox);
        }

        if (curLevelData.hasExtras)
            Toolbox.GameplayScript.environmentExtras[CurLevelData.environmentNumber].SetActive(true);

    }


    private void Update()
    {        
        if (spawnArmy)
        {
            if (Toolbox.GameplayScript.onTutorial)
            {
                time -= Time.deltaTime;

                if (time <= 0)
                {
                    GameObject obj = Instantiate(Toolbox.GameplayScript.GetPlayerArmyObj(), Toolbox.GameplayScript.startSpawnPoint.position, Toolbox.GameplayScript.startSpawnPoint.rotation);
                    Toolbox.GameplayScript.AddPlayerArmy(obj.GetComponent<CharacterHandler>());
                    obj.SetActive(true);
                    obj.GetComponent<CharacterHandler>().SetAutoMove(Toolbox.GameplayScript.areaListners[0].transform);

                    curArmySpawned++;
                    time = spawnDelay;

                    if (curArmySpawned >= startPlayerSpawnAmount)
                    {

                        spawnArmy = false;
                    }
                }
            }
            else
            {
                if (Toolbox.GameplayScript.doneInitialization)
                {
                    GameObject obj = Instantiate(Toolbox.GameplayScript.GetPlayerArmyObj(), Toolbox.GameplayScript.startSpawnPoint.position, Toolbox.GameplayScript.startSpawnPoint.rotation);
                    Toolbox.GameplayScript.AddPlayerArmy(obj.GetComponent<CharacterHandler>());
                    obj.SetActive(true);
                    obj.GetComponent<CharacterHandler>().SetAutoMove(Toolbox.GameplayScript.areaListenersList[0].transform);

                    curArmySpawned++;

                    if (curArmySpawned >= startPlayerSpawnAmount)
                    {
                        spawnArmy = false;
                    }
                }
            }
        }
        
        
    }
}
