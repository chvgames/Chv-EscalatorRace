using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTownIntersection : MonoBehaviour
{
    [field: SerializeField] public Transform town1SpawnPosition;
    [field: SerializeField] public Transform town2SpawnPosition;
    [field: SerializeField] public Transform townNearEnd;
    [field: SerializeField] public List<RoadHandler> roadHandlers;
}
