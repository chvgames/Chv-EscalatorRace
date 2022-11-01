using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDListner : MonoBehaviour {
    public static HUDListner instance;
    public GameplayScript gameplayScript;
    public Text timeTxt;
    public Text score;
    public Text timerText;
    public TextMeshProUGUI tutorialTapHoldStatus;
    public GameObject uiParent;
    public GameObject fightBtn;
    public GameObject tapMove;
    public GameObject holdMove;
    public static bool run = false;

    [Header("POWERUPS")]
    public TextMeshProUGUI dividePowerup;
    public TextMeshProUGUI slowPowerup;
    public TextMeshProUGUI speedPowerup;
    public TextMeshProUGUI shieldPowerup;
    public Animator divideAnimator;
    public Animator slowAnimator;
    public Animator speedAnimator;
    public Animator shieldAnimator;

    private int timer;
    Coroutine release;

    void Awake() {
        instance = this;
        Toolbox.Set_HudListner(this.GetComponent<HUDListner>());
        //AdsManager.instance.RequestBannerWithSpecs( Tapdaq.TDMBannerSize.TDMBannerStandard, Tapdaq.TDBannerPosition.Top);
        run = false;
    }

    private void OnEnable()
    {
        //timer = Toolbox.DB.prefs.CurrentTimer;
        //timerText.text = Toolbox.DB.prefs.CurrentTimer.ToString();
        //StartCoroutine(Timer());
    }

    public void DisableHUD() 
    {        
        uiParent.SetActive(false);
    }

    public void EnableHUD()
    {
        uiParent.SetActive(true);
        
    }

    private void Start()
    {
        if (!Toolbox.GameplayScript.onTutorial && !Toolbox.GameplayScript.onMainMenu)
        {
            dividePowerup.text = Toolbox.DB.prefs.DivideImmunityStack.ToString();
            slowPowerup.text = Toolbox.DB.prefs.SlowStack.ToString();
            speedPowerup.text = Toolbox.DB.prefs.SpeedStack.ToString();
            shieldPowerup.text = Toolbox.DB.prefs.ShieldStack.ToString();
        }
    }

    private void Update()
    {
        score.text =        Toolbox.GameplayScript.totalPlayersAvailable.ToString();

        //if (Input.GetKeyDown(KeyCode.Space))
        //    run = true;
        //else if (Input.GetKeyUp(KeyCode.Space))
        //    run = false;
    }

    #region POWERUPS

    public void DividePowerup()
    {
        if (Toolbox.GameplayScript.useDividePowerup || Toolbox.DB.prefs.DivideImmunityStack == 0)
            return;

        Toolbox.DB.prefs.DivideImmunityStack--;
        Toolbox.GameplayScript.useDividePowerup = true;
        divideAnimator.SetTrigger("usePowerup");
        dividePowerup.text = Toolbox.DB.prefs.DivideImmunityStack.ToString();
    }

    public void SlowPowerup()
    {
        if (Toolbox.GameplayScript.useSlowPowerup || Toolbox.DB.prefs.SlowStack == 0)
            return;

        Toolbox.DB.prefs.SlowStack--;
        Toolbox.GameplayScript.useSlowPowerup = true;
        slowAnimator.SetTrigger("usePowerup");
        slowPowerup.text = Toolbox.DB.prefs.SlowStack.ToString();
    }

    public void SpeedPowerup()
    {
        if (Toolbox.GameplayScript.useSpeedPowerup || Toolbox.DB.prefs.SpeedStack == 0)
            return;

        Toolbox.DB.prefs.SpeedStack--;
        Toolbox.GameplayScript.useSpeedPowerup = true;
        speedAnimator.SetTrigger("usePowerup");
        speedPowerup.text = Toolbox.DB.prefs.SpeedStack.ToString();
    }

    public void ShieldPowerup()
    {
        if (Toolbox.GameplayScript.useShieldPowerup || Toolbox.DB.prefs.ShieldStack == 0)
            return;

        Toolbox.DB.prefs.ShieldStack--;
        Toolbox.GameplayScript.useShieldPowerup = true;
        shieldAnimator.SetTrigger("usePowerup");
        shieldPowerup.text = Toolbox.DB.prefs.ShieldStack.ToString();
    }

    #endregion

    IEnumerator Timer()
    {
        if (Toolbox.GameplayScript.onTutorial)
            yield break;

        while(timer > 0)
        {
            yield return new WaitForSeconds(1f);

            if (gameplayScript.areaListners[gameplayScript.currentArea].isBossArea)
                yield break;

            if (gameplayScript.levelFailed || gameplayScript.levelCompleted)
                yield break;

            timer -= 1;
            timerText.text = timer.ToString();

            yield return null;
        }

        gameplayScript.LevelFailHandling();
    }

    public void SetLvlTxt(string _str) {

        
    }

    public void Press_Pause() {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
        Toolbox.GameManager.Instantiate_PauseMenu();
    }

    public void Press_Camera()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
    }
    public void Press_ControlChange()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
    }

    public void Press_Settings()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        Toolbox.GameManager.Instantiate_SettingsMenu();
    }

    public void PressRun() {
        //score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
        //Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        if (Toolbox.GameplayScript.onTutorial)
        {
            if (!Toolbox.GameplayScript.doneTapMove)
                release = StartCoroutine(StopTimeForTutorial(0.1f));
            else if (!Toolbox.GameplayScript.doneHoldMove)
                release = StartCoroutine(StopTimeForTutorial(1f));
        }

        Toolbox.Soundmanager.audioo.clip = Toolbox.Soundmanager.Play;
        Toolbox.Soundmanager.audioo.Play();
        Toolbox.Soundmanager.audioo.loop = true;
        run = true;
        
    }

    #region TUTORIAL

    IEnumerator StopTimeForTutorial(float starTime)
    {
        float currentTime = starTime;

        tutorialTapHoldStatus.text = "Release your finger";

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            yield return null;
        }

        Time.timeScale = 0f;
    }

    #endregion

    public void ReleaseRun()
    {
        //score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
        
        if (Toolbox.GameplayScript.onTutorial)
        {
            if (release != null)
                StopCoroutine(release);

            if (Time.timeScale == 0f)
                Time.timeScale = 1f;

            if (!Toolbox.GameplayScript.doneTapMove)
            {
                tapMove.SetActive(false);
                tutorialTapHoldStatus.gameObject.SetActive(false);
                tutorialTapHoldStatus.text = "Hold to Run";
                holdMove.SetActive(true);
                tutorialTapHoldStatus.gameObject.SetActive(true);
                Toolbox.GameplayScript.doneTapMove = true;
            }
            else if (!Toolbox.GameplayScript.doneHoldMove)
            {
                holdMove.SetActive(false);
                tutorialTapHoldStatus.gameObject.SetActive(false);
                Toolbox.GameplayScript.doneHoldMove = true;
            }
        }

        Toolbox.Soundmanager.audioo.loop = false;
        Toolbox.Soundmanager.audioo.Play();
        //Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        run = false;
        
    }

    public void OnPressFight() {
        //score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
        Toolbox.GameplayScript.StartFight();
        fightBtn.SetActive(false);
    }

}
