using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSelect : MonoBehaviour
{
    public UIManager uiManager;

    //object to be instantiated
    public GameObject abilityDisplayReward;

    //so that AbilityDisplayReward can deselect everything else when it is selected
    public List<AbilityDisplayReward> listReward = new List<AbilityDisplayReward>();

    //to send the ability to playerInventory
    public PlayerManager playerManager;

    public Button confirmSelection;



    private void Start() {
        confirmSelection.onClick.AddListener(addToInventory);
    }

    //adds the selected ability to inventory then leaves to UIManager's SceneName
    public void addToInventory() {
        //index will be used to know which ability from zoneRewards to send to AbilityInventory
        int index = 0;
        bool aSelectionIsMade=false;
        foreach(AbilityDisplayReward traversal in listReward) {
            //once it reaches a selected one add the ability to inventory
            if(traversal.selected == true) {
                aSelectionIsMade = true;
                GameObject temp = uiManager.zone.abilityRewardPool[index];
                temp.transform.parent = playerManager.abilityInventory.transform;
            }
            index++;
        }
        if (aSelectionIsMade) {
            //Debug.Log("as election is made");
            //deletes all displayws
            foreach (AbilityDisplayReward toBeDeleted in listReward) {
                Destroy(toBeDeleted.gameObject);
            }
            //saves the zone
            SaveSystem.saveZone(uiManager.zone);
            //saves map
            uiManager.saveMapSave();
            //then clears the list to be reinitialized in another zone
            listReward.Clear();
            //once leaves zone mark it as outside zone
            uiManager.inZone = false;
            //loads map
            uiManager.loadScene();

            //resets UI
            uiManager.pausePlayBtn.gameObject.SetActive(false);
            uiManager.timeControlHidden.hidden = true;
        }
    }
    //Displays the abilities and greys out the Button.
    public void displayAbilities() {
        //creates reward Displays
        for(int i = 0; i < uiManager.zone.abilityRewardPool.Count; i++) {
            AbilityDisplayReward rewardDisplay = Instantiate(abilityDisplayReward).GetComponent<AbilityDisplayReward>();
            //make it a child
            rewardDisplay.transform.parent = gameObject.transform;
            listReward.Add(rewardDisplay);
            //to connect this to the reward
            rewardDisplay.rewardSelect = this;
            //gets the ability from zone
            Ability temp = uiManager.zone.abilityRewardPool[i].GetComponent<Ability>();
            rewardDisplay.ability = temp;
            rewardDisplay.abilityName.text = temp.abilityName;
            rewardDisplay.description.text = temp.description;
            //sets the scale for some reason if I dont do this the scale is set to 167
            rewardDisplay.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        //greys out the button;
        greyOutBtn();
        //the button will be ungreyed out in AbilityRewardDisplayScript
    }

    public void greyOutBtn() {
        Image image = confirmSelection.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
    }
    public void unGreyOutBtn() {
        Image image = confirmSelection.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
