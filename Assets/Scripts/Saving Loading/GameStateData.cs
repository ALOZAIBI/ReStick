using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


[System.Serializable]

public class GameStateData : MonoBehaviour
{
    //What map player is currently doing. When you load a map save the string.
    //The string will be used to load the map When player opens the game
    public string mapName;

    public bool inProgress;
}
