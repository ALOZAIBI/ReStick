using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSelectItem : MonoBehaviour
{

    public GameObject itemDisplayReward;

    //so that AbilityDisplayReward can deselect everything else when it is selected
    public List<ItemDisplay> listItemReward = new List<ItemDisplay>();

    //to send the reward to inventory
    public PlayerManager playerManager;

    public Button confirmSelection;

    private void Start() {
        confirmSelection.onClick.AddListener(addToInventory);
    }
    //adds the selected ability to inventory
    public void addToInventory() {
        //index will be used to know which item from zoneRewards to send to itemInventory
        int index = 0;
        bool aSelectionIsMade = false;
        foreach (ItemDisplay traversal in listItemReward) {
            //once it reaches a selected one add the item to inventory
            if (traversal.selected == true) {
                aSelectionIsMade = true;
                //We add the item to inventory
                GameObject item = Instantiate(UIManager.singleton.itemFactory.objectFromName(traversal.item.itemName));
                item.transform.parent = playerManager.itemInventory.transform;

            }
            index++;
        }
        if (aSelectionIsMade) {
            //Debug.Log("as election is made");
            //deletes all displayws
            foreach (ItemDisplay toBeDeleted in listItemReward) {
                Destroy(toBeDeleted.gameObject);
                //goes to next step in gameWonScreen
            }
            //saves the zone
            SaveSystem.saveZone(UIManager.singleton.zone);
            //saves characters in map
            UIManager.singleton.saveMapSave();
            //then carries on in gameWonScreen
            UIManager.singleton.gameWonScreen.displayContents();
            //clears the list to be reinitialized in another zone
            listItemReward.Clear();
        }
        //SaveSystem.setRewardProgress(1);
    }
    //Displays the abilities and greys out the Button.
    public void displayItems() {
        Debug.Log("DISPLAY ITEMS CAKLLED");
        //hides contents of gameWonScreen so that rewards are displayed first
        UIManager.singleton.gameWonScreen.contents.SetActive(false);
        //creates reward Displays
        for (int i = 0; i < 3; i++) {
            Debug.Log("ITEM RTEWAARD DISPLY INSRTANT");
            ItemDisplay rewardDisplay = Instantiate(itemDisplayReward).GetComponent<ItemDisplay>();
            rewardDisplay.reward = true;
            //make it a child
            rewardDisplay.transform.parent = gameObject.transform;
            listItemReward.Add(rewardDisplay);
            //to connect this to the reward
            rewardDisplay.rewardSelect = this;

            Item temp = UIManager.singleton.itemFactory.randomItem().GetComponent<Item>();

            rewardDisplay.item = temp;
            Debug.Log("THIS IS THE ITEM MYLEAGUE");
            //rewardDisplay.itemName.text = temp.itemName;
            //rewardDisplay.itemDescription.text = temp.description;
            //sets the scale for some reason if I dont do this the scale is set to 167
            rewardDisplay.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        //greys out the button;
        greyOutItemBtn();
        //the button will be ungreyed out in AbilityRewardDisplayScript
    }

    public void greyOutItemBtn() {
        Image image = confirmSelection.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
    }
    public void unGreyOutItemBtn() {
        Image image = confirmSelection.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
