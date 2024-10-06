using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscBonusRewarder : MonoBehaviour, RewarderInterface
{
    public List<MiscBonusDisplay> displays = new List<MiscBonusDisplay>();

    public HideUI hideUI;

    public void hideRewards() {
        hideUI.hidden = true;
    }

    public void receiveReward() {
        MiscBonus selected=null;
        //Find which miscBonus was selected
        foreach (MiscBonusDisplay traversal in displays) {
            if (traversal.selected == true) {
                selected = traversal.bonus;
                //Add the miscBonus to the player's inventory
            }
        }

        //Apply the reward depending on the type of miscBonus

        RewardManager.singleton.rewardToApply = selected.gameObject;

        switch ((int)selected.type) {
            //If it's somethign that requires selecting a character
            case (int)MiscBonus.myType.HP:
                //Add to gold
            case(int)MiscBonus.myType.XP:
                //We don't do anything we will use the rewardToApply from within rewardmanager to pick the character
                //RewardManager.singleton.rewardToApply = selected.gameObject;
                break;
            case (int)MiscBonus.myType.Gold:
                //Add gold
                UIManager.singleton.playerParty.gold += (int)selected.bonusAmt;
                break;

            case (int)MiscBonus.myType.Life:
                //Add life
                UIManager.singleton.playerParty.lifeShards += (int)selected.bonusAmt;
                break;
        }

    }

    public void setUpRewards() {
        foreach (MiscBonusDisplay display in displays) {
            display.init(UIManager.singleton.miscBonusFactory.randomMiscBonus());
        }
        showRewards();
    }

    public void showRewards() {
        RewardManager.singleton.greyOutConfirmBtn();
        foreach (MiscBonusDisplay display in displays) {
            display.highlight();
        }
        hideUI.hidden = false;
    }
}
