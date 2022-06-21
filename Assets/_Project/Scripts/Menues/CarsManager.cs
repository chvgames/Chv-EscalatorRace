using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsManager : MonoBehaviour
{
    public GameObject[] carsBuybtn;
    // Start is called before the first frame update
    void Start()
    {
        CheckBoughtCars();
        Debug.Log(Toolbox.DB.prefs.CarsUnlocked[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPressBuy(int i)
    {
        Toolbox.DB.prefs.CarsUnlocked[i] = true;
        CheckBoughtCars();
    }
    public void CheckBoughtCars()
    {
        for (int j = 0; j < carsBuybtn.Length; j++)
        {
            if (Toolbox.DB.prefs.CarsUnlocked[j] == true)
            {
                carsBuybtn[j].SetActive(false);
            }
            else
            {
                carsBuybtn[j].SetActive(true);
            }
        }
    }
}
