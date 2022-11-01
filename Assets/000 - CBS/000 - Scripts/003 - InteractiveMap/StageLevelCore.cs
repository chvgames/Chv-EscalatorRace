using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StageLevelCore : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private List<StageMapController> stageMapController;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private LayerMask mask;

    [Header("CAMERA PANNING")]
    public Camera mainCamera;
    [SerializeField] private float groundZ;
    public float maxY;
    public float minY;

    [Header("LOADER")]
    [SerializeField] private TextMeshProUGUI triviaTMP;
    [SerializeField] private float speedAnimationTyper;
    [TextArea] [SerializeField] private List<string> loadingStatsList; 

    //  ====================================

    private GameActions inputActions;

    Vector2 hit_position = Vector2.zero;
    Vector2 current_position = Vector2.zero;
    Vector2 camera_position = Vector2.zero;
    Vector3 target_position;

    private bool initializeTouch;
    bool flag = false;

    private RaycastHit2D levelHit;
    private GameObject currentSelectedMap;
    private bool releaseLevelSelect;

    //  ====================================

    private Coroutine trivia;

    //  ====================================


    private void Awake()
    {
        loadingScreen.SetActive(true);

        inputActions = new GameActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Map.TouchContact.started -= _ => StartInitializeTouch(true);
        inputActions.Map.TouchContact.canceled -= _ => StartInitializeTouch(false);
        inputActions.Disable();
    }

    private void Start()
    {
        inputActions.Map.TouchContact.started += _ => StartInitializeTouch(true);
        inputActions.Map.TouchContact.canceled += _ => StartInitializeTouch(false);

        StartCoroutine(InstantiateMaps());
    }

    private void Update()
    {
        MoveCamera();
        SelectLevels();
    }

    #region INITIALIZE DATA

    IEnumerator InstantiateMaps()
    {
        trivia = StartCoroutine(TriviaShower());

        int currentMapCount = 1;

        for (int a = 0; a < stageMapController.Count; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                stageMapController[a].levelsList[b].LevelCore = this;
                stageMapController[a].levelsList[b].StarCount = Toolbox.DB.prefs.StarsLevel[currentMapCount - 1];
                stageMapController[a].levelsList[b].Unlocked = Toolbox.DB.prefs.UnlockedLevel[currentMapCount - 1];
                stageMapController[a].levelsList[b].StageNumber = currentMapCount;
                yield return StartCoroutine(stageMapController[a].levelsList[b].DataInitialize());

                currentMapCount++;

                yield return null;
            }

            yield return null;
        }

        if (trivia != null)
            StopCoroutine(trivia);

        loadingScreen.SetActive(false);
    }

    IEnumerator TriviaShower()
    {
        triviaTMP.text = "";

        int currentIndex = Random.Range(0, loadingStatsList.Count);

        foreach (char c in loadingStatsList[currentIndex])
        {
            triviaTMP.text += c;

            yield return new WaitForSecondsRealtime(speedAnimationTyper);

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        trivia = StartCoroutine(TriviaShower());
    }

    #endregion

    #region CONTROL

    private void StartInitializeTouch(bool value) => initializeTouch = value;

    private Vector2 TouchMovePosition() => inputActions.Map.TouchPosition.ReadValue<Vector2>();

    private Vector2 TouchDeltaMove() => inputActions.Map.TouchDelta.ReadValue<Vector2>();

    public void BackButton() => Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, true, 0);

    #endregion

    #region MOVE CAMERA

    private void MoveCamera()
    {
        if (currentSelectedMap != null)
            return;

        #region UNITY

        if (Input.GetMouseButtonDown(0))
        {
            hit_position = Input.mousePosition;
            camera_position = mainCamera.transform.position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            hit_position = Vector2.zero;
            camera_position = Vector2.zero;
            current_position = Vector2.zero;
        }

        if (Input.GetMouseButton(0))
        {
            current_position = Input.mousePosition;

            if (current_position != hit_position)
            {
                Drag();
                flag = true;
            }
        }

        if (flag)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(0f, Mathf.Clamp(target_position.y, minY, maxY), -10f), Time.deltaTime * 5f);
            if (mainCamera.transform.position == target_position || mainCamera.transform.position.y == minY || mainCamera.transform.position.y == maxY)//reached?
            {
                flag = false;// stop moving
            }
        }

        #endregion

        #region ANDROID

        //if (initializeTouch)
        //{
        //    if (hit_position == Vector2.zero)
        //    {
        //        hit_position = TouchMovePosition();
        //        camera_position = mainCamera.transform.position;
        //    }

        //    current_position = TouchMovePosition();

        //    if (current_position != hit_position)
        //    {
        //        Drag();
        //        flag = true;
        //    }
        //}
        //else
        //{
        //    hit_position = Vector2.zero;
        //    camera_position = Vector2.zero;
        //    current_position = Vector2.zero;
        //}

        //if (flag)
        //{
        //    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, new Vector3(0f, Mathf.Clamp(target_position.y, minY, maxY), -10f), Time.deltaTime * 5f);
        //    if (mainCamera.transform.position == target_position)//reached?
        //    {
        //        flag = false;// stop moving
        //    }
        //}

        #endregion
    }

    private void Drag()
    {
        // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
        // anyways.  
        Vector2 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        // Invert direction to that terrain appears to move with the mouse.
        direction = direction * -3.5f;

        target_position = camera_position + direction;
    }

    #endregion

    #region SELECT LEVELS

    private void SelectLevels()
    {

        #region UNITY

        levelHit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, mask);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (levelHit && levelHit.transform.tag == "Levels")
            {
                if (!levelHit.transform.gameObject.GetComponent<LevelMapController>().Unlocked)
                    return;

                levelHit.transform.gameObject.GetComponent<LevelMapController>().SelectedButton();
                currentSelectedMap = levelHit.transform.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentSelectedMap == null)
                return;

            if (EventSystem.current.IsPointerOverGameObject() || !levelHit)
            {
                currentSelectedMap.GetComponent<LevelMapController>().UnselectedButton();
                currentSelectedMap = null;
            }
            else if (levelHit.transform.gameObject == currentSelectedMap)
            {
                //  PROCEED TO NEXT STAGE
                levelHit.transform.gameObject.GetComponent<LevelMapController>().PutangInaPlayTheGameNa();
            }
        }

        #endregion

        #region ANDROID

        //levelHit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(TouchMovePosition()), Vector2.zero, Mathf.Infinity, mask);

        //if (initializeTouch)
        //{
        //    releaseLevelSelect = true;

        //    if (levelHit && levelHit.transform.tag == "Levels")
        //    {
        //        if (!levelHit.transform.gameObject.GetComponent<LevelMapController>().Unlocked)
        //            return;

        //        levelHit.transform.gameObject.GetComponent<LevelMapController>().SelectedButton();
        //        currentSelectedMap = levelHit.transform.gameObject;
        //    }
        //}
        //else
        //{
        //    if (!releaseLevelSelect)
        //        return;

        //    if (currentSelectedMap == null)
        //        return;

        //    if (EventSystem.current.IsPointerOverGameObject() || !levelHit)
        //    {
        //        currentSelectedMap.GetComponent<LevelMapController>().UnselectedButton();
        //        currentSelectedMap = null;
        //    }
        //    else if (levelHit.transform.gameObject == currentSelectedMap)
        //    {
        //        //  PROCEED TO NEXT STAGE
        //    }
        //}

        #endregion
    }

    #endregion
}
