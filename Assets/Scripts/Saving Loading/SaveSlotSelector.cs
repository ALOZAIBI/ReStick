using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotSelector : MonoBehaviour {
    public Button startSlot1Btn;
    public Button startSlot2Btn;
    public Button startSlot3Btn;

    // Start is called before the first frame update
    void Start() {
        startSlot1Btn.onClick.AddListener(startSlot1);
        startSlot2Btn.onClick.AddListener(startSlot2);
        startSlot3Btn.onClick.AddListener(startSlot3);

    }

    public void startSlot1() {
        UIManager.saveSlot = "slot1";

        //if it's a new Save add a character to player party making playerparty have 3 children
        //if in map load characters in map else load characters in world
        if (SaveSystem.loadGameState()) {
            UIManager.singleton.loadMapSave();
        }
        //by default playerParty has only 2 children so if it's still just 2 children (loadGameState didn't add a character) load the characters
        else if
            (UIManager.singleton.playerParty.transform.childCount == 2) {
            UIManager.singleton.loadWorldSave();
            SaveSystem.loadCharactersInWorld();
        }
    }

    public void startSlot2() {
        UIManager.saveSlot = "slot2";

        //if it's a new Save add a character to player party making playerparty have 3 children
        //if in map load characters in map else load characters in world
        if (SaveSystem.loadGameState()) {
            UIManager.singleton.loadMapSave();
        }
        //by default playerParty has only 2 children so if it's still just 2 children (loadGameState didn't add a character) load the characters
        else if
            (UIManager.singleton.playerParty.transform.childCount == 2) {
            UIManager.singleton.loadWorldSave();
            SaveSystem.loadCharactersInWorld();
        }
    }

    public void startSlot3() {
        UIManager.saveSlot = "slot3";

        //if it's a new Save add a character to player party making playerparty have 3 children
        //if in map load characters in map else load characters in world
        if (SaveSystem.loadGameState()) {
            UIManager.singleton.loadMapSave();
        }
        //by default playerParty has only 2 children so if it's still just 2 children (loadGameState didn't add a character) load the characters
        else if
            (UIManager.singleton.playerParty.transform.childCount == 2) {
            UIManager.singleton.loadWorldSave();
            SaveSystem.loadCharactersInWorld();
        }
    }
}
