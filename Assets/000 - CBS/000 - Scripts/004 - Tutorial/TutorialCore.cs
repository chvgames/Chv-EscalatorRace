using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCore : MonoBehaviour
{
    [Header("WAYPOINTS")]
    [SerializeField] private Transform playersSpawnPoint;
    [SerializeField] private List<AreaListner> areaListeners;

    [Header("PLAYERS")]
    [SerializeField] private GameObject playerObj;

    [Header("CAMERA")]
    [SerializeField] private CamHandler cameraListener;

    [Header("UI")]
    [SerializeField] private GameObject tapToMovePanelObj;
    [SerializeField] private GameObject holdToMovePanelObj;

    //  =================================

    private bool doneTapMove;
    private bool doneHoldMove;

    private List<CharacterHandler> activePlayerCharacters;

    //  =================================

    private void Awake()
    {
        SpawnPlayerOnStart();
    }

    #region PLAYER

    private void SpawnPlayerOnStart()
    {
        GameObject player = Instantiate(playerObj, playersSpawnPoint.position, playersSpawnPoint.rotation);
        activePlayerCharacters.Add(player.GetComponent<CharacterHandler>());
        player.SetActive(true);
        player.GetComponent<CharacterHandler>().SetAutoMove(areaListeners[0].transform);
    }

    #endregion
}
