using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMapController : MonoBehaviour
{
    [field: SerializeField] public List<LevelMapController> levelsList;
    [field: SerializeField] public Transform endPos;
}
