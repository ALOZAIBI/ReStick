using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRewarder : MonoBehaviour, RewarderInterface {

    public List<ItemDisplay> displays = new List<ItemDisplay>();

    public HideUI hideUI;
    public void receiveReward() {
        //index will be used to know which item from zoneRewards to send to itemInventory
        int index = 0;
        bool aSelectionIsMade = false;
        foreach (ItemDisplay traversal in displays) {
            //once it reaches a selected one add the item to inventory
            if (traversal.selected == true) {
                aSelectionIsMade = true;
                //We add the item to inventory
                UIManager.singleton.itemFactory.addRequestedItemToInventory(traversal.item.itemName);

            }
            index++;
        }
        if (aSelectionIsMade) {
            //This is now called from rewardManager since it will use the hidden values before resetting them
            //hideRewards();
        }
    }


    public void setUpRewards() {
        foreach(ItemDisplay display in displays) {
            display.init(UIManager.singleton.itemFactory.randomItem().GetComponent<Item>());
        }
        showRewards();
    }
    public void hideRewards() {
        hideUI.hidden = true;
    }

    public void showRewards() {
        RewardManager.singleton.greyOutConfirmBtn();
        foreach(ItemDisplay display in displays) {
            display.highlight();
        }
        hideUI.hidden = false;
    }
}
