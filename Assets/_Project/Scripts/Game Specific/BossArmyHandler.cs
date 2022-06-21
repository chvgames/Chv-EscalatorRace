using UnityEngine;

public class BossArmyHandler : MonoBehaviour
{
    bool spawnArmy = false;
    int curArmySpawned = 0;
    int totalArmyAmounts = 0;
    float spawnDelay = 0.1f;
    float time = 0;

    public GameObject armyPrefab;

    public Transform[] standPoint;
    int index = 0;

    public void SpawnBossArmy(int _army) {

        totalArmyAmounts = _army;
        spawnArmy = true;
    }

    private void Update()
    {
        if (spawnArmy)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                GameObject obj = Instantiate(armyPrefab, standPoint[index].position, standPoint[index].rotation);
                Toolbox.GameplayScript.AddBossArmy(obj.GetComponent<CharacterHandler>());
                obj.SetActive(true);

                curArmySpawned++;
                time = spawnDelay;
                
                index++;

                if (index >= standPoint.Length) {

                    index = 0;
                }

                if (curArmySpawned >= totalArmyAmounts)
                {
                    spawnArmy = false;
                    Toolbox.GameplayScript.totalBossPlayersAvailable = curArmySpawned;
                }
            }
        }
    }

}
