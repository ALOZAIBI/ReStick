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

    //the text displaying
    public GameObject sold;
    public GameObject buy;

    public Button self;
    public bool purchased;

    public int price;

    public int index;

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
        background.color = ColorPalette.singleton.getTypeColor(ability.abilityType);
        unHighlight();
        //change alpha to 0.1 if purchased
        if (purchased) {
            Color tempColor = background.color;
            tempColor.a = 0.1f;
            background.color = tempColor;
        }
    }
    //selected click again to buy
    public virtual void highlight() {
        if (!purchased) {
            Color temp = background.color;
            temp.a = 1f;
            background.color = temp;
            buy.SetActive(true);
        }
    }
    public virtual void unHighlight() {
        if (!purchased) {
            Color temp = background.color;
            temp.a = 0.7f;
            background.color = temp;
            buy.SetActive(false);
        }
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
            foreach (CharacterDisplayShop deSelect in UIManager.singleton.shopScreen.listCharacters) {
                if (deSelect != this) {
                    deSelect.selected = false;
                    deSelect.unHighlight();
                }
            }
            foreach (ItemDisplay itemDisplay in UIManager.singleton.shopScreen.listItems) {
                if (itemDisplay != this) {
                    itemDisplay.selected = false;
                    itemDisplay.showThatNextClickWillPurchase.SetActive(false);
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
                UIManager.singleton.abilityFactory.addRequestedAbilityToInventory(ability.abilityName);
                //Since Shops are only in maps we save to map
                UIManager.singleton.saveMapSave();
                //save shop PurchaseInfo
                SaveSystem.saveShopAbilitiesItemsAndPurchaseInfo(UIManager.singleton.shopScreen.shop);
            }
        }
        
    }
    private void markPurchased() {
        purchased = true;
        //marks the corresponding index to purchased
        UIManager.singleton.shopScreen.shop.abilitiyPurchased[index] = true;
        displaySold();
        UIManager.singleton.updateGoldtextToCurrent();
    }

    public void displaySold() {
        Color tempColor = background.color;
        tempColor.a = 0.1f;
        background.color = tempColor;
        sold.SetActive(true);
        //rotate sold randomly along the z axis
        sold.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-40,40));
    }
}
