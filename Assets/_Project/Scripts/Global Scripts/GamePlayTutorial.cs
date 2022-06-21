using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayTutorial : MonoBehaviour
{
    public Text textbox;
    public string[] AllDescriptions;
    public GameObject Pointers;

    private int no = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        textbox.text = AllDescriptions[no];
        Pointers.transform.GetChild(no).gameObject.SetActive(true);
    }

    public void OnPress_Tutorial()
    {
        no++;
        if (no < AllDescriptions.Length)
        {
            
            textbox.text = AllDescriptions[no];
            Pointers.transform.GetChild(no - 1).gameObject.SetActive(false);
            Pointers.transform.GetChild(no).gameObject.SetActive(true);
            SizeAnimation _imgObj = Pointers.transform.GetChild(no).gameObject.AddComponent<SizeAnimation>();
            _imgObj.minSize = 0.9f;
            _imgObj.maxSize = 1f;
            _imgObj.speed = 0.007f;

        }
        else this.gameObject.SetActive(false);
    }
}

