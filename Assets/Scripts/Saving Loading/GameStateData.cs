using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


[System.Serializable]

public class GameStateData
{
    //What map player is currently doing. When you load a map save the string.
    //The string will be used to load the map When player opens the game
    public string mapName;

    //wether player is in map or not
    public bool inMap;

    public GameStateData(string mapName,bool inMap) {
        this.mapName = mapName;
        this.inMap = inMap;
    }
    
    public void loadMapOrWorldScene() {
        if (inMap) {
            SceneManager.LoadScene(mapName);
        }
        else {
            SceneManager.LoadScene("World");
        }
    }
    //this is needed for SaveSystem to be able to deserialize it
    public GameStateData() { }
    public void initNewSave() {
        //add random character to playerParty
        UIManager temp = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        temp.characterFactory.addRandomCharacterAsChild(temp.playerParty.gameObject);
        //then saves gameState
        SaveSystem.saveGameState("", false);
        //then saves the new character
        temp.saveWorldSave();
        //then loads world
        SceneManager.LoadScene("World");
    }
}
