using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour {
    public static RewardManager singleton;

    public AbilityRewarder abilityRewarder;
    public ItemRewarder itemRewarder;

    public Button confirmBtn;
    public HideUI confirmBtnHidden;

    //To immediately add the abiliy/item to a specific character
    public CharacterDisplayRewardApplication characterDisplay;
    public Transform charDisplayPanel;
    public HideUI rewardApplicationHidden;
    //Could be item/ability
    public GameObject rewardToApply;

    private void Start() {
        singleton = this;
        confirmBtn.onClick.AddListener(confirmReward);

    }

    //Returns true if a reward has been displayed
    public bool displayRewards() {
        bool thereIsReward = false;
        float random = Random.Range(0, 100);
        //If the zone I'm in forces a reward
        if (UIManager.singleton.zone.forceReward) {
            //Get Either an ability or an item
            if (random < 50) {
                abilityRewarder.setUpRewards();
                thereIsReward = true;
            }
            else {
                itemRewarder.setUpRewards();
                thereIsReward = true;
            }
        }
        //Different percentc chances to get different rewards
        else {
            //20% chance to get a reward
            if (random < 20) {
                //Get Either an ability or an item(for now , later will add more types of rewards)
                if (Random.Range(0, 100) < 50) {
                    abilityRewarder.setUpRewards();
                    thereIsReward = true;
                }
                else {
                    itemRewarder.setUpRewards();
                    thereIsReward = true;
                }
            }
        }

        confirmBtnHidden.hidden = !thereIsReward;
        return thereIsReward;
    }

    public void confirmReward() {
        //Whatever isn't hidden is the current reward thing so we receive it
        if(!abilityRewarder.hideUI.hidden)
            abilityRewarder.receiveReward();
        else if (!itemRewarder.hideUI.hidden)
            itemRewarder.receiveReward();

        confirmBtnHidden.hidden = true;

        startRewardApplication();
    }

    //After confirming a reward, display the charDisplays to pick who to add the reward to
    public void startRewardApplication() {
        rewardApplicationHidden.hidden = false;
        //We retrieve the reward object
        initRewardObject();
        //We reset the hidden values
        abilityRewarder.hideRewards();
        itemRewarder.hideRewards();

        //We instantiate all the char displays in the chardisplay panel
        foreach (Transform child in UIManager.singleton.playerParty.transform) {
            if (child.CompareTag("Character")) {
                CharacterDisplayRewardApplication display = Instantiate(characterDisplay, charDisplayPanel);
                display.character = child.GetComponent<Character>();
                display.GetComponent<Button>().onClick.AddListener(() => applyRewardToCharacter(display.character, display));
            }
        }
    }

    public void applyRewardToCharacter(Character character,CharacterDisplayRewardApplication display) {
        Debug.Log("I'll reward this charcater !!" + character.name);
        //if the rewardToApply is an ability
        if (rewardToApply.GetComponent<Ability>() != null) {
            character.addAbility(rewardToApply.GetComponent<Ability>());
        }
        //if the rewardToApply is an item
        else if (rewardToApply.GetComponent<Item>() != null) {
            character.addItem(rewardToApply.GetComponent<Item>());
        }

        //To prevent new thing notification when we decite to add immediately to character
        UIManager.singleton.playerParty.setNewStuffNotifications();
        //Maybe add animation of updating the sprite when archetyper changes dues to item or ability
        endRewardApplication();
    }

    private void initRewardObject() {
        Debug.Log("Init rewawrd obvject called");
        //To figure out what type of reward it was, we can test the hidden value
        if (!abilityRewarder.hideUI.hidden) {
            //The reward object is the last ability added to inventory
            rewardToApply = UIManager.singleton.playerParty.abilityInventory.transform.GetChild(UIManager.singleton.playerParty.abilityInventory.transform.childCount - 1).gameObject;
        }
        else if (!itemRewarder.hideUI.hidden) {
            //The reward object is the last item added to inventory
            rewardToApply = UIManager.singleton.playerParty.itemInventory.transform.GetChild(UIManager.singleton.playerParty.itemInventory.transform.childCount - 1).gameObject;
        }

        Debug.Log(abilityRewarder.hideUI.hidden + " " + itemRewarder.hideUI.hidden);
    }

    public void endRewardApplication() {
        //Deletes the chardisplays
        foreach(Transform toDelete in charDisplayPanel) {
            Destroy(toDelete.gameObject);
        }
        rewardApplicationHidden.hidden = true;


        //Display progression(Next step after rewards) + Save
        UIManager.singleton.gameWonScreen.displayProgression();
        UIManager.singleton.gameWonScreen.saveProgression();
    }

    public void greyOutConfirmBtn() {
        Image image = confirmBtn.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
    }
    public void unGreyOutConfirmBtn() {
        Image image = confirmBtn.GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }
}
