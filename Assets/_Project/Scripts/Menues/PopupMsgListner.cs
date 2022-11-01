using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMsgListner : MonoBehaviour
{
    public Text msgTxt;
    public GameObject unlockBtn;
    public bool isPopupMsg = false;
    public void UpdateMsg(string str) {

        msgTxt.text = str;
    }

    public void OnEnable()
    {
        if(isPopupMsg) Invoke("PauseTime", 1.2f);
    }

    public void OnPress_Close() {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Destroy(this.gameObject);
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void OnPress_UnlockMega() {

        OnPress_Close();
    }

    public void OnPress_PrivacyPolicy()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
        Application.OpenURL(Constants.privacyPolicy);
    }

    public void OnPress_AgreeOfConsent()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

        Toolbox.DB.prefs.FirstRun = false;
        Toolbox.GameManager.LoadScene(Constants.sceneIndex_Tutorial, false, 0);
        Destroy(this.gameObject);
    }

    public void PauseTime()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    public void Shop()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
        Toolbox.GameManager.Instantiate_Shop();
        OnPress_Close();
    }
}
