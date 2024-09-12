using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Displays progression
//Calls rewardManager
public class GameWonScreen : MonoBehaviour
{

    public Button goToNextZoneBtn;
    public Button goBackToMapBtn;
    public GameObject contents;
    public int rewardEveryNZone;



    private void Start() {
        goBackToMapBtn.onClick.AddListener(goToMap);
    }

    public void zoneWon() {
        goBackToMapBtn.gameObject.SetActive(false);
        contents.SetActive(false);
        if (!RewardManager.singleton.displayRewards()) {
            UIManager.singleton.openShop();
            //If no reward received just display progression
            displayProgression();
            //And save progression
            saveProgression();
        }
            
    }

    //Saves progression (Zone completion + Whatever rewards we got + however the character's progressed)
    public void saveProgression() {
        SaveSystem.saveZone(UIManager.singleton.zone);
        UIManager.singleton.saveMapSave();
    }

    //contents i.e character progression gold earned, and buttons
    public void displayProgression() {
        RewardManager.singleton.abilityRewarder.hideUI.hidden = true;
        RewardManager.singleton.itemRewarder.hideUI.hidden = true;
        contents.SetActive(true);
        goBackToMapBtn.gameObject.SetActive(true);
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
