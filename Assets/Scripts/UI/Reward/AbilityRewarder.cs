using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityRewarder : MonoBehaviour, RewarderInterface
{
    public List<AbilityDisplayReward> displays = new List<AbilityDisplayReward>();


    public HideUI hideUI;

    public void applyReward() {

        //index will be used to know which ability from zoneRewards to send to AbilityInventory
        int index = 0;
        bool aSelectionIsMade = false;
        foreach (AbilityDisplayReward traversal in displays) {
            //once it reaches a selected one add the ability to inventory
            if (traversal.selected == true) {
                aSelectionIsMade = true;
                UIManager.singleton.abilityFactory.addRequestedAbilityToInventory(traversal.ability.abilityName);
            }
            index++;
        }
        if (aSelectionIsMade) {
            //Debug.Log("as election is made");
            //deletes all displayws
            foreach (AbilityDisplayReward toBeDeleted in displays) {
                Destroy(toBeDeleted.gameObject);
                //goes to next step in gameWonScreen
            }
            //Saves zone Completion + MapSave after this has been from gameWonScreen
            hideRewards();
        }
    }

    public void setUpRewards() {
        foreach(AbilityDisplayReward display in displays) {
            display.init(UIManager.singleton.abilityFactory.randomAbility());
        }
        showRewards();
    }
    public void hideRewards() {
        hideUI.hidden = true;
    }


    public void showRewards() {
        RewardManager.singleton.greyOutConfirmBtn();
        foreach(AbilityDisplayReward display in displays) {
            display.highlight();
        }
        hideUI.hidden = false;
    }



}
