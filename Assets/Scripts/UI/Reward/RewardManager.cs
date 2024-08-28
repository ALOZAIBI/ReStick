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
        //Whatever isn't hidden is the current reward thing so we apply it
        if(!abilityRewarder.hideUI.hidden)
            abilityRewarder.applyReward();
        else if (!itemRewarder.hideUI.hidden)
            itemRewarder.applyReward();

        confirmBtnHidden.hidden = true;
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
