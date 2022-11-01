using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelMapController : MonoBehaviour
{

    public int StarCount
    {
        get => starCount;
        set => starCount = value;
    }

    public int StageNumber
    {
        get => stageNumber;
        set => stageNumber = value;
    }

    public bool Unlocked
    {
        get => isUnlocked;
        set => isUnlocked = value;
    }

    public StageLevelCore LevelCore
    {
        get => levelCore;
        set => levelCore = value;
    }

    //  ================================

    [SerializeField] private StageData stageData;
    [SerializeField] private StageData.Stage stage;
    [SerializeField] private bool isBossBattle;
    [SerializeField] private List<GameObject> stars;
    [SerializeField] private TextMeshProUGUI stageNumberTMP;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    [Header("ANIMATION")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;

    //  =================================

    StageLevelCore levelCore;
    int buttonSRLT;
    private int starCount;
    private int stageNumber;
    private bool isUnlocked; 
    private SpriteRenderer level;
    private SpriteRenderer sprite;

    //  =================================

    private void Awake()
    {
        level = GetComponent<SpriteRenderer>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public IEnumerator DataInitialize()
    {
        stageNumberTMP.text = stageNumber.ToString();

        if (Unlocked)
            level.sprite = unlockedSprite;
        else
            level.sprite = lockedSprite;

        for (int a = 0; a < starCount; a++)
        {
            stars[a].SetActive(true);

            yield return null;
        }

        if (stageNumber == Toolbox.DB.prefs.CurrentLevel + 1)
            LevelCore.mainCamera.transform.position = new Vector3(0f, Mathf.Clamp(transform.position.y, LevelCore.minY, LevelCore.maxY), -10f);
    }

    public void PutangInaPlayTheGameNa()
    {
        stageData.CurrentStage = stage;
        stageData.currentStageNumber = stageNumber;
        stageData.isBossFight = isBossBattle;

        Toolbox.GameManager.LoadScene(Constants.sceneIndex_Game_1, true, 0);
    }

    #region ANIMATION

    public void SelectedButton()
    {
        if (buttonSRLT != 0)
            LeanTween.cancel(buttonSRLT);

        buttonSRLT = LeanTween.value(gameObject, SetSRColor, sprite.color, selectedColor, 0.25f).setEase(LeanTweenType.easeInOutCubic).id;
    }

    public void UnselectedButton()
    {
        if (buttonSRLT != 0)
            LeanTween.cancel(buttonSRLT);

        buttonSRLT = LeanTween.value(gameObject, SetSRColor, sprite.color, unselectedColor, 0.25f).setEase(LeanTweenType.easeInOutCubic).id;
    }

    private void SetSRColor(Color color)
    {
        sprite.color = color;
    }

    #endregion
}
