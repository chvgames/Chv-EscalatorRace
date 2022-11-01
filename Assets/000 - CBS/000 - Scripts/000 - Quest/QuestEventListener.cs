using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestEventListener : MonoBehaviour
{
    [Header("BUTTONS")]
    [SerializeField] private Button questClaimButton;
    [SerializeField] private Button dailyLoginButton;
    [SerializeField] private List<Button> socMedsButton;

    [Header("SLIDER")]
    [SerializeField] private Slider progressSlider;

    [Header("TEXT")]
    [SerializeField] private Text gatherTroopsText;
    [SerializeField] private Text finishLevelText;
    [SerializeField] private Text defeatBossText;

    [Header("DONE OBJECTS")]
    [SerializeField] private GameObject questDone;
    [SerializeField] private GameObject dailyLoginDone;
    [SerializeField] private GameObject shareWithDone;
    [SerializeField] private GameObject gatherTroopsDone;
    [SerializeField] private GameObject levelsDone;
    [SerializeField] private GameObject bossesDone;

    //  ============================================

    private List<bool> questList;
    int total = 0;

    //  ============================================

    private void OnEnable()
    {
        questList = new List<bool>();
        questList.Add(false);
        questList.Add(false);
        questList.Add(false);
        questList.Add(false);
        questList.Add(false);

        QuestChecker();
        DailyLoginChecker();
        SocMedChecker();
        GatherTroopsChecker();
        LevelsDone();
        BossDone();
        CheckProgressBar();
    }

    private void CheckProgressBar()
    {
        
        foreach(bool status in questList)
        {
            if (status)
                total += 1;
        }

        progressSlider.value = total / 5f;
    }

    private void QuestChecker()
    {
        if (DateTime.Now >= Toolbox.DB.prefs.LastDoneQuestTime?.AddHours(24))
        {
            total = 0;
            Toolbox.DB.prefs.LastDoneQuestTime = null;
            Toolbox.DB.prefs.LastDailyLogin = null;
            Toolbox.DB.prefs.DoneSocMedShare = false;
            Toolbox.DB.prefs.GatheredTroops = 0;
            Toolbox.DB.prefs.LevelsDone = 0;
            Toolbox.DB.prefs.BossesFightDone = 0;
        }

        if (Toolbox.DB.prefs.LastDoneQuestTime == null)
        {
            questClaimButton.interactable = true;
            questDone.SetActive(false);
        }
        else
        {
            if (total >= 5 && DateTime.Now >= Toolbox.DB.prefs.LastDoneQuestTime)
            {
                questClaimButton.interactable = false;
                questDone.SetActive(true);
            }
            else if (total < 5 && DateTime.Now >= Toolbox.DB.prefs.LastDoneQuestTime?.AddHours(24))
            {
                questClaimButton.interactable = true;
                questDone.SetActive(false);
            }
        }
    }

    private void DailyLoginChecker()
    {
        if (Toolbox.DB.prefs.LastDailyLogin == null)
        {
            dailyLoginButton.interactable = true;
            dailyLoginDone.SetActive(false);

            questList[0] = false;
        }
        else
        {
            if (Toolbox.DB.prefs.LastDailyLogin?.Date.AddHours(24) < DateTime.Now &&
                Toolbox.DB.prefs.LastDailyLogin > DateTime.Now)
            {
                dailyLoginButton.interactable = true;
                dailyLoginDone.SetActive(false);

                questList[0] = false;
            }
            else if (Toolbox.DB.prefs.LastDailyLogin?.Date.AddHours(24) >= DateTime.Now)
            {
                dailyLoginButton.interactable = false;
                dailyLoginDone.SetActive(true);

                questList[0] = true;
            }
            else
            {
                dailyLoginButton.interactable = true;
                dailyLoginDone.SetActive(false);

                questList[0] = false;
            }
        }
    }

    private void SocMedChecker()
    {
        if (Toolbox.DB.prefs.DoneSocMedShare)
        {
            foreach (Button buttons in socMedsButton)
                buttons.interactable = false;
            shareWithDone.SetActive(true);

            questList[1] = true;
        }
        else
        {
            foreach (Button buttons in socMedsButton)
                buttons.interactable = true;
            shareWithDone.SetActive(false);

            questList[1] = false;
        }
    }

    private void GatherTroopsChecker()
    {
        gatherTroopsText.text = Toolbox.DB.prefs.GatheredTroops.ToString() + "/300";
        if (Toolbox.DB.prefs.GatheredTroops >= 300)
        {
            gatherTroopsDone.SetActive(true);

            questList[2] = true;
        }
        else
        {
            gatherTroopsDone.SetActive(false);

            questList[2] = false;
        }
    }

    private void LevelsDone()
    {
        finishLevelText.text = Toolbox.DB.prefs.LevelsDone.ToString() + "/10";
        if (Toolbox.DB.prefs.LevelsDone >= 10)
        {
            levelsDone.SetActive(true);

            questList[3] = true;
        }
        else
        {
            levelsDone.SetActive(false);

            questList[3] = false;
        }
    }

    private void BossDone()
    {
        defeatBossText.text = Toolbox.DB.prefs.BossesFightDone.ToString() + "/2";
        if (Toolbox.DB.prefs.BossesFightDone >= 2)
        {
            bossesDone.SetActive(true);
            questList[4] = true;
        }
        else
        {
            bossesDone.SetActive(false);
            questList[4] = false;
        }
    }

    #region BUTTONS

    public void QuestClaim()
    {
        if (total < 5)
        {
            Toolbox.GameManager.InstantiatePopup_Message("You still didn't finish all of your quest! Finish it all to claim your quest");
        }
        else if (total >= 5)
        {
            Toolbox.DB.prefs.LastDoneQuestTime = DateTime.Now;

            //  TOKEN HERE
            Toolbox.GameplayScript.IncrementGoldCoins(50000);
        }
    }

    public void DailyLogin()
    {
        Toolbox.DB.prefs.LastDailyLogin = DateTime.Now;
        Debug.Log(Toolbox.DB.prefs.LastDailyLogin);
        DailyLoginChecker();
        CheckProgressBar();
    }

    public void ShareSocmed()
    {
        if (Application.isEditor)
        {
            Toolbox.DB.prefs.DoneSocMedShare = true;
            SocMedChecker();
            DailyLoginChecker();
        }
        else
            new NativeShare().SetText("Start playing Escalator with me!").SetUrl("https://play.google.com/store/apps/details?id=com.chvdev.escalator.race.simulator").SetCallback(
                (result, shareTarget) => ProcessShareResult(result));
    }

    private void ProcessShareResult(NativeShare.ShareResult result)
    {
        if (result == NativeShare.ShareResult.Shared)
        {
            Toolbox.DB.prefs.DoneSocMedShare = true;
            SocMedChecker();
            DailyLoginChecker();
        }

        else if (result == NativeShare.ShareResult.NotShared)
            Toolbox.GameManager.InstantiatePopup_Message("You didn't share it :( \r please share it.");

        else if(result == NativeShare.ShareResult.Unknown)
            Toolbox.GameManager.InstantiatePopup_Message("Please only use Facebook, Instagram, and Twitter to share!");
    }

    #endregion

    public void BackButton()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        Destroy(gameObject);
    }
}
