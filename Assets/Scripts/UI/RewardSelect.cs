using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSelect : MonoBehaviour
{
    //need to add grid display to this screen so that the AbilityDisplayReward is displayed correctly
    public UIManager uiManager;

    //object to be instantiated
    public GameObject abilityDisplayReward;

    //so that AbilityDisplayReward can deselect everything else when it is selected
    public List<AbilityDisplayReward> listReward = new List<AbilityDisplayReward>();

    //to send the ability to playerInventory
    public PlayerManager playerManager;

    public Button confirmtSelection;

    private void Start() {
        confirmtSelection.onClick.AddListener(addToInventory);
    }

    //adds the selected ability to inventory
    public void addToInventory() {
        foreach(AbilityDisplayReward traversal in listReward) {
            //once it reaches a selected one add the ability to inventory
            if(traversal.selected == true) {
                playerManager.abilityInventory.Add(traversal.ability);
                //deletes all displayws
                foreach(AbilityDisplayReward toBeDeleted in listReward) {
                    Destroy(toBeDeleted);
                }
                //then clears the list to be reinitialized in another zone
                listReward.Clear();
                //then closes the screen
                this.gameObject.SetActive(false);
            }
        }
    }
    public void displayAbilities() {
        //creates 3 reward Displays
        for(int i = 0; i < 3; i++) {
            AbilityDisplayReward rewardDisplay = Instantiate(abilityDisplayReward).GetComponent<AbilityDisplayReward>();
            //make it a child
            rewardDisplay.transform.parent = gameObject.transform;
            listReward.Add(rewardDisplay);
            //to connect this to the reward
            rewardDisplay.rewardSelect = this;
            Ability temp = uiManager.zone.abilityRewardPool[i];
            rewardDisplay.ability = temp;
            rewardDisplay.abilityName.text = temp.abilityName;
            rewardDisplay.description.text = temp.description;
            //sets the scale for some reason if I dont do this the scale is set to 167
            rewardDisplay.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
