using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour {
    public static RewardManager singleton;

    public AbilityRewarder abilityRewarder;

    public Button confirmBtn;
    public HideUI confirmBtnHidden;

    private void Start() {
        singleton = this;
        confirmBtn.onClick.AddListener(confirmReward);
    }

    //Returns true if a reward has been displayed
    public bool displayRewards() {
        bool thereIsReward = false;
        //If the zone I'm in forces a reward
        if (UIManager.singleton.zone.forceReward) {
            //Get Either an ability or an item
            if (Random.Range(0, 100) < 100) {
                abilityRewarder.setUpRewards();
                thereIsReward = true;
            }
            else {
                //displayItemRewards();
            }
        }
        //Different percentc chances to get different rewards
        else {
            //For now however always get an ability
            abilityRewarder.setUpRewards();
            thereIsReward = true;
        }

        confirmBtnHidden.hidden = !thereIsReward;
        return thereIsReward;
    }

    public void confirmReward() {
        if(abilityRewarder.gameObject.activeSelf)
            abilityRewarder.applyReward();

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
