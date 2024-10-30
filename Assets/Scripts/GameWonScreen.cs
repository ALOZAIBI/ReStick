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
    public CharacterDisplayProgression characterDisplayProgression;
    private int numOfDisplaysCreated = 0;
    [SerializeField]private int numOfDisplaysDone = 0;
    private List<Character> charactersLeveledUp = new List<Character>();

    public int rewardEveryNZone;

    [SerializeField] private float rewardChance=50;
    [SerializeField] private float shopChance=20;


    [ButtonInvoke(nameof(declareADisplayDone))] public bool countUpCharDisplayDone;
    private void Start() {
        goBackToMapBtn.onClick.AddListener(goToMap);
    }

    public void zoneWon() {
        goBackToMapBtn.gameObject.SetActive(false);
        contents.SetActive(false);
        if (UIManager.singleton.zone.forceReward) {
            RewardManager.singleton.displayRewards();
            return;
        }
        float random = Random.Range(0, 100);

        //50% chance to get a reward
        if (random < rewardChance) {
            RewardManager.singleton.displayRewards();
        }
        //20% chance to get shop
        else if (random < rewardChance + shopChance) {
            UIManager.singleton.openShop();
        }
        else {
            displayProgression();
            saveProgression();
        }
            
    }

    //Saves progression (Zone completion + Whatever rewards we got + however the character's progressed)
    public void saveProgression() {
        SaveSystem.saveZone(UIManager.singleton.zone);
        UIManager.singleton.saveMapSave();
    }

    public void declareADisplayDone() {
        numOfDisplaysDone++;
        if (numOfDisplaysDone == numOfDisplaysCreated) {
            //Will display the first character that has leveld up 
            UIManager.singleton.characterInfoScreen.viewCharacterFullScreen(charactersLeveledUp[0]);
            UIManager.singleton.characterInfoScreen.time = UIManager.singleton.characterInfoScreen.transitionTime;
            UIManager.singleton.charInfoScreenHidden.hidden = false;
            UIManager.singleton.characterInfoScreen.showUpgradeStats();
        }
    }
    //contents i.e character progression gold earned, and buttons
    public void displayProgression() {
        RewardManager.singleton.abilityRewarder.hideUI.hidden = true;
        RewardManager.singleton.itemRewarder.hideUI.hidden = true;
        RewardManager.singleton.miscBonusRewarder.hideUI.hidden = true;
        contents.SetActive(true);
        createProgressionDisplays();
        goBackToMapBtn.gameObject.SetActive(true);
    }
    private void deleteDisplays() {
        foreach (Transform child in contents.transform) {
            if(!child.CompareTag("DontDelete"))
                Destroy(child.gameObject);
        }
        numOfDisplaysCreated = 0;
    }
    private void createProgressionDisplays() {
        deleteDisplays();
        //Foreach character that was placed create a progression display for it
        foreach(Character character in UIManager.singleton.playerParty.getCharacters()) {
            if (character.dropped) {
                CharacterDisplayProgression display =  Instantiate(characterDisplayProgression, contents.transform);
                display.character = character;
                numOfDisplaysCreated++;
                //If character has leveled up, add it to the list
                if (character.zsLevel != character.level) {
                    charactersLeveledUp.Add(character);
                }
            }
        }
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
