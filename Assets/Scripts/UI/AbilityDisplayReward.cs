using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityDisplayReward : MonoBehaviour
{

    public Ability ability;
    //to be able to deselect everything else when this is selected
    public RewardSelect rewardSelect;
    //wether this is selected or not
    public bool selected;
    //used to color what is selected
    public Image background;

    public Image rarity;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;

    public Button self;

    private void Start() {
        self.onClick.AddListener(select);


        if (ability.rarity == (int)Ability.RaritiesList.Common)
            rarity.color = ColorPalette.singleton.commonRarity;
        if (ability.rarity == (int)Ability.RaritiesList.Rare) {
            rarity.color = ColorPalette.singleton.rareRarity;
        }
        if (ability.rarity == (int)Ability.RaritiesList.Epic) {
            rarity.color = ColorPalette.singleton.epicRarity;
        }
        if (ability.rarity == (int)Ability.RaritiesList.Legendary) {
            rarity.color = ColorPalette.singleton.legendaryRarity;
        }
        background.color = ColorPalette.singleton.getIndicatorColor(ability.abilityType);

        unHighlight();
    }

    public void highlight() {
        Color temp = background.color;
        temp.a = 1f;
        background.color = temp;
    }
    public void unHighlight() {
        Color temp = background.color;
        temp.a = 0.7f;
        background.color = temp;
    }

    private void select() {
        Debug.Log("Selected");
        selected = true;
        highlight();
        //deselects alll others
        foreach(AbilityDisplayReward deSelect in rewardSelect.listReward) {
            if(deSelect != this) {
                deSelect.selected = false;
                deSelect.unHighlight();
            }
        }
        //and ungreys out the confirmselection button
        rewardSelect.unGreyOutBtn();
    }
}
