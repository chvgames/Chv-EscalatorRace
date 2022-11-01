using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentArea : MonoBehaviour
{
    [field: SerializeField] public AreaListner areaListner { get; private set; }
    [field: SerializeField] public Transform spawnPoint { get; private set; }
    [field:SerializeField] public Transform spawnPointTwo { get; private set; }
    [field: SerializeField] public Animator gateAnimator { get; private set; }

    [field: Header("END POINTS")]
    [field: SerializeField] public Transform finishPoint { get; private set; }
    [field: SerializeField] public GameObject jumpTrigger { get; private set; }
    [field: SerializeField] public GameObject finalRoad { get; private set; }

    [field: Header("BOSS BATTLE")]
    [field: SerializeField] public Transform bossSpawnPoint { get; private set; }
    [field: SerializeField] public Transform fightPoint { get; private set; }
    [field: SerializeField] public BossArmyHandler bossArmyHandler { get; private set; }

    [field: Header("TOWN END")]
    [field: SerializeField] public RoadHandler roadHandlerEnd;
}
