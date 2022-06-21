using UnityEngine;

public class AreaHandler : MonoBehaviour
{
    public Transform standPoint;
    public int inPlayersCount = 0;
    public Transform camPosition;

    private int curPlayerIndex = 0;
    public int curRowIndex = 0;
    public int curColumnIndex = -1;

    public int maxRowLimit = 5;
    public int maxColumnLimit = 10;

    public float xoffset = 0.1f;
    public float zoffset = -0.1f;

    public void AddInPlayerArmy() {

        inPlayersCount++;
    }

    public Vector3 GetArmyPosition() {

        curPlayerIndex++;
        curColumnIndex++;

        if (curColumnIndex >= maxColumnLimit) {

            curRowIndex++;
            curColumnIndex = 0;
        }

        if (curRowIndex >= maxRowLimit) { 
        
            //Next Level --> Khichdi
        }

        return standPoint.position + new Vector3(xoffset * curColumnIndex, 0, zoffset * curRowIndex);
    }
}
