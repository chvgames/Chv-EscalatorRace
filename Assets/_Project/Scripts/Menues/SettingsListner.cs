using UnityEngine;
using UnityEngine.UI;

public class SettingsListner: MonoBehaviour {

    public GameObject soundBtn;
    public GameObject musicBtn;
    public GameObject soundOff;
    public GameObject musicOff;



    public Toggle soundToggle;
    public Toggle musicToggle;
    public Toggle shadowToggle;

    public GameObject [] panels;
    public Image [] controlsBtn;
    public Image [] graphicsBtn;

    public Sprite bigSelectOption;
    public Sprite bigDeselectOption;

    private void OnEnable()
    {
        //Press_Control(Toolbox.DB.prefs.IsSteerControl);
        //Press_Graphics(Toolbox.DB.prefs.GraphicsVal);
        soundToggle.isOn = Toolbox.DB.prefs.GameAudio;
        musicToggle.isOn = Toolbox.DB.prefs.GameMusic;
        //shadowToggle.isOn = Toolbox.DB.prefs.HasShadows;
        if (!soundToggle.isOn) 
            soundOff.SetActive(true); 
        if (!musicToggle.isOn) 
            musicOff.SetActive(true); 
    }

    public void OnPress_Close()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        Destroy(this.gameObject);
    }
    public void OnMusicToggle()
    {      
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        if (Toolbox.DB.prefs.GameMusic = musicToggle.isOn)
            musicOff.SetActive(false);
        else if (!Toolbox.DB.prefs.GameMusic)
            musicOff.SetActive(true);
        Toolbox.Soundmanager.UpdateMusicStatus();
    }

    public void OnSoundToggle()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        if(Toolbox.DB.prefs.GameAudio = soundToggle.isOn) 
            soundOff.SetActive(false); 
        else if(!Toolbox.DB.prefs.GameAudio) 
            soundOff.SetActive(true); 
        Toolbox.Soundmanager.UpdateSoundStatus();
    }
    public void OnShadowToggle()
    {
        Toolbox.DB.prefs.HasShadows = shadowToggle.isOn;
    }

    public void Press_Control(bool _isSteering) {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Toolbox.DB.prefs.IsSteerControl = _isSteering;

        if (_isSteering)
        {

            controlsBtn[0].sprite = bigDeselectOption;
            controlsBtn[1].sprite = bigSelectOption;
        }
        else {

            controlsBtn[0].sprite = bigSelectOption;
            controlsBtn[1].sprite = bigDeselectOption;
        }

    }
    public void Press_Panel (int _val)
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == _val)
                panels[i].SetActive(true);            
            else
                panels[i].SetActive(false);
        }

        switch (_val)
        {
            case 0:
                Press_Control(Toolbox.DB.prefs.IsSteerControl);
                break;

            case 1:
                Press_Graphics(Toolbox.DB.prefs.GraphicsVal);
                break;

            default:
                break;
        }
    }

    public void Press_Graphics(int _val)
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Toolbox.DB.prefs.GraphicsVal = _val;

        for (int i = 0; i < graphicsBtn.Length; i++)
        {
            if (i == _val)
                graphicsBtn[i].sprite = bigSelectOption;
            else
                graphicsBtn[i].sprite = bigDeselectOption;
        }
    }

    public void Press_RateUs() {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Application.OpenURL(Constants.appLink);
    }

    public void Press_PrivacyPolicy()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Application.OpenURL(Constants.privacyPolicy);
    }
}
