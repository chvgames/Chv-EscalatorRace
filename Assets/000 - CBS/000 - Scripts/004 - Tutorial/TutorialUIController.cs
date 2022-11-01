using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIController : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private List<Sprite> tutorials;
    [TextArea] [SerializeField] private List<string> tutorialText;
    [SerializeField] private Text tutorial;
    [SerializeField] private Image tutorialImg;

    [Header("BUTTONS")]
    [SerializeField] private GameObject continueObj;
    [SerializeField] private GameObject nextObj;
    [SerializeField] private GameObject previousObj;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button previousBtn;

    //  ===============================

    int currentIndex;

    //  ===============================

    private void OnEnable()
    {
        CheckImageText();
        CheckIndex();
    }

    public void NextPreviousButton(bool next)
    {
        if (next)
        {
            currentIndex += 1;
        }
        else
        {
            currentIndex -= 1;
        }

        CheckImageText();
        CheckIndex();
    }

    public void Continue() => Destroy(gameObject);

    private void CheckIndex()
    {
        if (currentIndex == 0 && tutorials.Count > 1)
        {
            nextObj.SetActive(true);
            previousObj.SetActive(true);
            continueObj.SetActive(false);

            previousBtn.interactable = false;
            nextBtn.interactable = true;
        }
        else if (currentIndex == tutorials.Count - 1 && tutorials.Count == 1)
        {
            previousObj.SetActive(true);
            nextObj.SetActive(false);
            continueObj.SetActive(true);

            previousBtn.interactable = false;
            nextBtn.interactable = false;
        }
        else if (currentIndex > 0 && currentIndex < tutorials.Count - 1)
        {
            nextObj.SetActive(true);
            previousObj.SetActive(true);
            continueObj.SetActive(false);

            previousBtn.interactable = true;
            nextBtn.interactable = true;
        }
        else if (currentIndex == tutorials.Count - 1)
        {
            previousObj.SetActive(true);
            nextObj.SetActive(false);
            continueObj.SetActive(true);

            previousBtn.interactable = true;
            nextBtn.interactable = false;
        }
    }

    private void CheckImageText()
    {
        tutorial.text = tutorialText[currentIndex];
        tutorialImg.sprite = tutorials[currentIndex];
    }
}
