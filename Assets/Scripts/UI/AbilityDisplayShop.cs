using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityDisplayShop : MonoBehaviour
{
    public Ability ability;
    //wether this is selected or not
    public bool selected;
    //used to color what is selected
    public Image background;

    public Image rarity;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI priceText;

    public Button self;
    public bool purchased;

    public int price;

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
        if (!selected) {
            Debug.Log("Selected");
            selected = true;
            highlight();
            //deselects alll others
            foreach (AbilityDisplayShop deSelect in UIManager.singleton.shopScreen.listAbilities) {
                if (deSelect != this) {
                    deSelect.selected = false;
                    deSelect.unHighlight();
                }
            }
        }
        //if already selected then clicked again
        else if (!purchased) {
            //if can afford
            if (UIManager.singleton.playerParty.gold >= price) {
                markPurchased();
                //deduct from player gold
                UIManager.singleton.playerParty.gold -= price;
                //add to inventroy
                Instantiate(ability, UIManager.singleton.playerParty.abilityInventory.transform);
                //Since Shops are only in maps we save to map
                UIManager.singleton.saveMapSave();
                //save shop PurchaseInfo
                SaveSystem.saveShopAbilitiesAndPurchaseInfo(UIManager.singleton.shopScreen.shop);
            }
        }
        
    }
    private void markPurchased() {
        purchased = true;
        //index relative to siblings
        int index = transform.GetSiblingIndex();
        //marks the corresponding index to purchased
        UIManager.singleton.shopScreen.shop.abilitiyPurchased[index] = true;
        //change color of display
    }
}
