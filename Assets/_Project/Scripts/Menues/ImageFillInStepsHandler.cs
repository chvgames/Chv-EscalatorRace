using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFillInStepsHandler : MonoBehaviour
{
    public GameObject [] fillImages;

    public void FillImage(float _val) {

        _val *= 10; // because _val will be in 0 - 1

        for (int i = 0; i < fillImages.Length; i++)
        {
            if (i <= _val)
                fillImages[i].SetActive(true);            
            else
                fillImages[i].SetActive(false);
        }
    }
}
