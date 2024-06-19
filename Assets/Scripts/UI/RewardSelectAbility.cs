using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSelectAbility : MonoBehaviour
{
    public UIManager uiManager;

    //object to be instantiated
    public GameObject abilityDisplayReward;

    //so that AbilityDisplayReward can deselect everything else when it is selected
    public List<AbilityDisplayReward> listAbilityReward = new List<AbilityDisplayReward>();

    //to send the reward to inventory
    public PlayerManager playerManager;

    public Button confirmSelection;



    private void Start() {
        confirmSelection.onClick.AddListener(addToInventory);
    }

    //adds the selected ability to inventory
    public void addToInventory() {
        //index will be used to know which ability from zoneRewards to send to AbilityInventory
        int index = 0;
        bool aSelectionIsMade=false;
        foreach(AbilityDisplayReward traversal in listAbilityReward) {
            //once it reaches a selected one add the ability to inventory
            if(traversal.selected == true) {
                aSelectionIsMade = true;
                uiManager.abilityFactory.addRequestedAbilityToInventory(traversal.ability.abilityName);
            }
            index++;
        }
        if (aSelectionIsMade) {
            //Debug.Log("as election is made");
            //deletes all displayws
            foreach (AbilityDisplayReward toBeDeleted in listAbilityReward) {
                Destroy(toBeDeleted.gameObject);
                //goes to next step in gameWonScreen
            }
            //saves the zone
            SaveSystem.saveZone(UIManager.singleton.zone);
            //saves characters in map
            UIManager.singleton.saveMapSave();
            //then carries on in gameWonScreen
            uiManager.gameWonScreen.displayContents();
        //clears the list to be reinitialized in another zone
        listAbilityReward.Clear();
        }
        //SaveSystem.setRewardProgress(1);
    }
    //Displays the abilities and greys out the Button.
    public void displayAbilities() {
        //hides contents of gameWonScreen so that rewards are displayed first
        uiManager.gameWonScreen.contents.SetActive(false);
        //creates reward Displays
        for(int i = 0; i < uiManager.zone.abilityRewardPool.Count; i++) {
            AbilityDisplayReward rewardDisplay = Instantiate(abilityDisplayReward).GetComponent<AbilityDisplayReward>();
            //make it a child
            rewardDisplay.transform.parent = gameObject.transform;
            listAbilityReward.Add(rewardDisplay);
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
        Debug.Log("Debug poolCount"+uiManager.zone.abilityRewardPool.Count);
        //greys out the button;
        greyOutAbilityBtn();
        //the button will be ungreyed out in AbilityRewardDisplayScript

        //Tutorial stuff
        uiManager.tutorial.beginChooseRewardTutorial();
    }

    public void greyOutAbilityBtn() {
        Image image = confirmSelection.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
    }
    public void unGreyOutAbilityBtn() {
        Image image = confirmSelection.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
