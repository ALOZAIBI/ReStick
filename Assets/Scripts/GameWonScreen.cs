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
    public int rewardEveryNZone;

    private void Start() {
        rewardSelectHidden = rewardSelect.GetComponent<HideUI>();
        //goToNextZoneBtn.onClick.AddListener(goToNextLevelIfPossible);
        goBackToMapBtn.onClick.AddListener(goToMap);
    }

    public void zoneWon() {
        if (SaveSystem.giveReward()) {
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
        //once leaves zone mark it as outside zone
        UIManager.singleton.inZone = false;
        //loads map and clears buffs then 
        UIManager.singleton.loadScene();
        //saves characters in map
        UIManager.singleton.saveMapSave();

        //resets UI
        UIManager.singleton.pausePlayBtn.gameObject.SetActive(false);
        UIManager.singleton.timeControlHidden.hidden = true;
    }
}
