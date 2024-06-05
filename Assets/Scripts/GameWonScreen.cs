using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWonScreen : MonoBehaviour
{
    public RewardSelectAbility rewardSelectAbility;
    public RewardSelectItem rewardSelectItem;
    private HideUI rewardSelectItemHidden;
    private HideUI rewardSelectAbilityHidden;
    public CharacterDisplay characterDisplay;
    public Button goToNextZoneBtn;
    public Button goBackToMapBtn;
    public GameObject contents;
    public int rewardEveryNZone;

    private void Start() {
        rewardSelectAbilityHidden = rewardSelectAbility.GetComponent<HideUI>();
        rewardSelectItemHidden = rewardSelectItem.GetComponent<HideUI>();
        //goToNextZoneBtn.onClick.AddListener(goToNextLevelIfPossible);
        goBackToMapBtn.onClick.AddListener(goToMap);
    }

    public void zoneWon() {
        //Random number between 0 and 100
        int random = UnityEngine.Random.Range(0, 100);
        //20% chance to get a reward
        if (random < 0) {
            displayAbilityRewards();
        }
        else if(random<100){
            Debug.Log("WILL REWRA ITEM");
            displayItemRewards();
        }
        else {
            displayContents();
        }
    }

    //contents i.e character progression gold earned, and buttons
    public void displayContents() {
        rewardSelectAbilityHidden.hidden = true;
        rewardSelectItemHidden.hidden = true;
        contents.SetActive(true);
    }
    private void displayAbilityRewards() {
        contents.SetActive(false);
        rewardSelectAbilityHidden.hidden = false;
        rewardSelectAbility.displayAbilities();
    }
    private void displayItemRewards() {
        contents.SetActive(false);
        rewardSelectItemHidden.hidden = false;
        rewardSelectItem.displayItems();
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
