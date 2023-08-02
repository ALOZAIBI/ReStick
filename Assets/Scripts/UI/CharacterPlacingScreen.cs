using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPlacingScreen : MonoBehaviour
{
    //the characterDisplay script handles the drag and wdrop stuff
    public UIManager uiManager;

    //the prefab to be instantiated
    public GameObject characterDisplay;

    public Button startBtn;

    private void Start() {
        startBtn.onClick.AddListener(startZone);
    }

    
    //displays the characters
    public void displayCharacters() {
        //deletes all created instances before recreating to account for dead characters etc..
        close();
        //loops through children of playerParty
        foreach (Transform child in uiManager.playerParty.transform) {
            //Debug.Log("Child" + child.name+child.tag);
            if (child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                //instantiates a charcaterDisplay
                CharacterDisplay display = Instantiate(characterDisplay).GetComponent<CharacterDisplay>();
                display.character = temp;
                //sets this display as a child 
                display.transform.parent = transform;
                //sets the scale for some reason if I dont do this the scale is set to 167
                display.gameObject.transform.localScale = new Vector3(1, 1, 1);
                if (!temp.alive) {
                    display.deathSkull.gameObject.SetActive(true);
                }
                
            }
        }
    }

    //deletes all the created instances and hides button and screen
    public void close() {
        foreach(Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        startBtn.gameObject.SetActive(false);
    }

    //starts the zone precondition = atleast 1 playerCharacter is in Map
    private void startZone() {
        //saves in Map to save the targetting strategies
        UIManager.singleton.saveMapSave();
        //unhides these 2
        uiManager.pausePlayBtn.gameObject.SetActive(true);
        uiManager.timeControlHidden.hidden = false;
        uiManager.pausePlay(false);
        //this has to be set to pause since we dont want it to be true when zone gameplay starts
        uiManager.wasPause = false;
        close();
        uiManager.zone.started = true;
        //does initRoundStart for playerParty Character's this is enough only on playerPartyCharacter's because the other character's will have initroundstart called anyways in the start function
        try {
            //Debug.Log("try");
            foreach (Transform temp in uiManager.playerParty.transform) {
                if (temp.tag == "Character") {
                    Character tempChar = temp.GetComponent<Character>();
                    tempChar.initRoundStart();
                }
            }
        }
        catch { }
        //hides the screen
        GetComponent<HideUI>().hidden = true;
        //Randomizes the CDs a bit on zoneStart so that the character won't throw everything all at once
        foreach(Transform child in uiManager.playerParty.transform) {
            if(child.tag == "Character") {
                Character temp = child.GetComponent<Character>();
                foreach(Ability ability in temp.abilities) {
                    ability.abilityNext = Random.Range(0.5f, 3);
                }
            }
        }
        //hides the placeable overlay in zone
        uiManager.zone.placeableOverlay.gameObject.SetActive(false);
    }

    //checks if zone is startable(if atleast 1 playerCharacter)
    private void zoneStartableHmm() {
        //loops through player characters
        foreach(Transform child in uiManager.playerParty.transform) {
            //if atleast 1 character is active display startBtn
            if (child.tag == "Character") {
                if (child.gameObject.activeSelf) {
                    startBtn.gameObject.SetActive(true);
                    return;
                }
            }
        }
        //otherwise hide startbtn
        startBtn.gameObject.SetActive(false);
    }



    private void Update() {
        zoneStartableHmm();
    }
}

