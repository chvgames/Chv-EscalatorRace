using UnityEngine;
using UnityEngine.UI;

public class HUDListner : MonoBehaviour {
    public static HUDListner instance;
    public Text timeTxt;
    public Text score;
    public GameObject uiParent;
    public GameObject fightBtn;
    public static bool run = false; 
    void Awake() {
        instance = this;
        Toolbox.Set_HudListner(this.GetComponent<HUDListner>());
        //AdsManager.instance.RequestBannerWithSpecs( Tapdaq.TDMBannerSize.TDMBannerStandard, Tapdaq.TDBannerPosition.Top);
        run = false;
        score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString(); 
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

    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    run = true;
        //else if (Input.GetKeyUp(KeyCode.Space))
        //    run = false;
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
        Toolbox.Soundmanager.audioo.clip = Toolbox.Soundmanager.Play;
        Toolbox.Soundmanager.audioo.Play();
        Toolbox.Soundmanager.audioo.loop = true;
        run = true;
        
    }

    public void ReleaseRun()
    {
        //score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
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
