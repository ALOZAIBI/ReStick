using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWonScreen : MonoBehaviour
{
    public RewardSelect rewardSelect;
    private HideUI rewardSelectHidden;
    public CharacterDisplay characterDisplay;
    public Button goToNextZoneBtn;
    public Button goBackToMapBtn;
    public GameObject contents;
    public int chanceToGetRewardPercent;

    private void Start() {
        rewardSelectHidden = rewardSelect.GetComponent<HideUI>();
        goToNextZoneBtn.onClick.AddListener(goToNextLevelIfPossible);
        goBackToMapBtn.onClick.AddListener(goToMap);
    }

    public void zoneWon() {
    //60% chance for a reward to happen AND IF ZONE WASN'T COMPLETED BEFORE
        int giveReward = UnityEngine.Random.Range(0, 100);
        if (giveReward <= chanceToGetRewardPercent&& !UIManager.singleton.zone.completed) {
            displayRewards();
        }
        else
            displayContents();
    }

    //contents i.e character progression gold earned, and buttons
    public void displayContents() {
        rewardSelectHidden.hidden = true;
        contents.SetActive(true);
    }
    private void displayRewards() {
        contents.SetActive(false);
        rewardSelectHidden.hidden = false;
        rewardSelect.displayAbilities();
    }
    //leaves to UIManager's SceneName
    public void goToMap() {
        //saves the zone
        SaveSystem.saveZone(UIManager.singleton.zone);
        //saves map
        UIManager.singleton.saveMapSave();
        //once leaves zone mark it as outside zone
        UIManager.singleton.inZone = false;
        //loads map
        UIManager.singleton.loadScene();

        //resets UI
        UIManager.singleton.pausePlayBtn.gameObject.SetActive(false);
        UIManager.singleton.timeControlHidden.hidden = true;
    }

    public void goToNextLevelIfPossible() {
        //saves the zone
        SaveSystem.saveZone(UIManager.singleton.zone);
        //saves map
        UIManager.singleton.saveMapSave();
        UIManager.singleton.sceneToLoad = GetNextScene(SceneManager.GetActiveScene().name);
        //removes characters from zone
        foreach (Transform child in UIManager.singleton.playerParty.transform) {
            if (child.tag == "Character") {
                child.gameObject.SetActive(false);
            }
        }
        //loads nextZone
        UIManager.singleton.loadScene();
    }
    //thanks chat gpt
    public  string GetNextScene(string initZoneName) {
        // Split the scene name into two parts: the zone and the number
        string[] parts = initZoneName.Split('-');

        // Check if the scene name is in the correct format
        if (parts.Length != 2) {
            throw new ArgumentException("Invalid scene name format. Expected format: ZoneX-Y");
        }

        string zone = parts[0];
        string numberString = parts[1];
        //Debug.Log("Zone:" + zone + "ns:" + numberString);
        // Parse the number and increment it by 1
        if (int.TryParse(numberString, out int number)) {
            number++;
            return zone + "-" + number.ToString();
        }

        throw new ArgumentException("Invalid scene name format. Number is not a valid integer.");
    }
}
