using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archetype : MonoBehaviour
{
    //The actual object will have the placeholder name the archetypeName will be the cool name to be used in game
    public string archetypeName;
    //Which prefab this archetype uses
    public int prefabIndex;

    //For each Point upgrade stat that many times
    //Out of 10
    public int PWR;
    public int MGC;
    public int INF;
    public int HP;
    public int AS;
    public int CDR;
    public int SPD;
    //Range is counted as 0
    public int Range;
    public int LS;

    private void Start() {
        if(archetypeName == "") {
            archetypeName = name;
        }
    }

}
