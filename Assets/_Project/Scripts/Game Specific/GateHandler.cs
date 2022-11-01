using UnityEngine;

public class GateHandler : MonoBehaviour
{
    //public SpriteRenderer renderer;

    float time = 0;
    public float changeTime = 5;
    public int type = 0;

    public Transform[] spawnPoint;
    public SpriteRenderer[] render; 

     //public Color goodColor = Color.blue;
     //public Color badColor = Color.red;
    
    int divideCharVal = 0;

    //public TextMesh text;


    private void Start()
    {
        time = changeTime;
    }
    private void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0) {

            time = changeTime;
            ChangeGate();
        }
    }

    public void ChangeGate() {

        type = Random.Range(0, 5);
        divideCharVal = 0;

        switch (type)
        {
            case 0:
                render[0].enabled = true;
                render[1].enabled = false;
                render[2].enabled = false;
                render[3].enabled = false;
                //text.text = "X 2";
                //text.color = goodColor;
                break;

            case 1:
                render[0].enabled = false;
                render[1].enabled = true;
                render[2].enabled = false;
                render[3].enabled = false;
                //text.text = "X 3";
                //text.color = goodColor;
                break;

            case 2:
                render[0].enabled = false;
                render[1].enabled = false;
                render[2].enabled = true;
                render[3].enabled = false;
                //text.text = "/3";
                //text.color = badColor;
                break;

            case 3:
                render[0].enabled = false;
                render[1].enabled = false;
                render[2].enabled = false;
                render[3].enabled = true;
                //text.text = "/2";
                //text.color = badColor;
                break;

            case 4:
                render[0].enabled = true;
                render[1].enabled = false;
                render[2].enabled = false;
                render[3].enabled = false;
                //text.text = "X 2";
                //text.color = goodColor;
                break;

            default:
                break;
        }
    }

    public void PerformAction(Transform _val) {

        //Debug.LogError("Performing = " + type);

        switch (type)
        {
            case 0:
                if (Toolbox.GameplayScript.totalPlayersAvailable >= 270)
                    return;

                int _point = Random.Range(0, spawnPoint.Length - 1);
                GameObject obj = Instantiate(_val.gameObject, spawnPoint[_point].position, spawnPoint[_point].rotation);
                Toolbox.GameplayScript.AddPlayerArmy(obj.GetComponent<CharacterHandler>());
                HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
                break;

            case 1:
                if (Toolbox.GameplayScript.totalPlayersAvailable >= 270)
                    return;

                int _point2 = Random.Range(0, spawnPoint.Length - 1);
                int _point3 = Random.Range(0, spawnPoint.Length - 1);

                GameObject obj1 = Instantiate(_val.gameObject, spawnPoint[_point2].position, spawnPoint[_point2].rotation);
                Toolbox.GameplayScript.AddPlayerArmy(obj1.GetComponent<CharacterHandler>());
                GameObject obj2 = Instantiate(_val.gameObject, spawnPoint[_point3].position, spawnPoint[_point2].rotation);
                Toolbox.GameplayScript.AddPlayerArmy(obj2.GetComponent<CharacterHandler>());
                HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();

                break;

            case 2:
                divideCharVal++;

                if (divideCharVal % 3 != 0 && !Toolbox.GameplayScript.useDividePowerup) {

                    _val.GetComponent<CharacterHandler>().Die();
                    Toolbox.GameplayScript.totalDeaths++;
                    HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
                }

                break;

            case 3:

                divideCharVal++;

                if (divideCharVal % 2 == 0 && !Toolbox.GameplayScript.useDividePowerup)
                {
                    _val.GetComponent<CharacterHandler>().Die();
                    Toolbox.GameplayScript.totalDeaths++;
                    HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
                }
                break;

            case 4:
                if (Toolbox.GameplayScript.totalPlayersAvailable >= 270)
                    return;

                int _point4 = Random.Range(0, spawnPoint.Length - 1);
                GameObject obj4 = Instantiate(_val.gameObject, spawnPoint[_point4].position, spawnPoint[_point4].rotation);
                Toolbox.GameplayScript.AddPlayerArmy(obj4.GetComponent<CharacterHandler>());
                HUDListner.instance.score.text = Toolbox.GameplayScript.totalPlayersAvailable.ToString();
                break;

            default:
                break;
        }
    }
}
