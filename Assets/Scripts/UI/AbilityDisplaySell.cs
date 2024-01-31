using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityDisplaySell: AbilityDisplayShop {

    //the text displaying
    public GameObject sell;

    private void Start() {
        priceText.text = price+ "";
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
    //selected click again to buy
    public override void highlight() {
        
            Color temp = background.color;
            temp.a = 1f;
            background.color = temp;
            sell.SetActive(true);
    }
    public override void unHighlight() {
        
        Color temp = background.color;
        temp.a = 0.7f;
        background.color = temp;
        sell.SetActive(false);
        
    }

    private void select() {
        if (!selected) {
            selected = true;
            highlight();
            //deselects alll others
            foreach (AbilityDisplaySell deSelect in UIManager.singleton.shopScreen.listSellableAbilities) {
                if (deSelect != this) {
                    deSelect.selected = false;
                    deSelect.unHighlight();
                }
            }
        }
        //if already selected then clicked again then sell it
        else{

            //add to player gold
            UIManager.singleton.playerParty.gold += price;
            //remove from inventory
            Destroy(ability.gameObject);

            //Change it's parent to the shop before destroying it
            //This is done since Destroy() is delayed and saveMapSave would save it before it is destroyed so we change it's parent away from the inventory
            //This way it wont be saved
            ability.transform.SetParent(UIManager.singleton.shopScreen.transform);

            //Refresh the shop view
            UIManager.singleton.shopScreen.displaySellScreen();
            //Since Shops are only in maps we save to map
            UIManager.singleton.saveMapSave();
        }
        
    }
}
