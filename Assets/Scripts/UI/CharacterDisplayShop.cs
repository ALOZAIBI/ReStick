using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterDisplayShop : MonoBehaviour {
    public Character character;

    [SerializeField] private Image characerPortrait;
    [SerializeField] private CharacterHealthBar healthBar;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Button self;
    public TextMeshProUGUI priceText;
    public int price;

    public bool selected;
    public bool purchased;    
    private void Start() {
        //price depends on how many characters the player has.
        price = 30*(UIManager.singleton.playerParty.transform.childCount-2);
        priceText.text = price + "";
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;

        self.onClick.AddListener(select);
        //change alpha to 0.3 if purchased
        if (purchased) {
            Color tempColor = characerPortrait.color;
            tempColor.a = 0.1f;
            characerPortrait.color = tempColor;
        }
    }

    private void select() {
        if (!selected) {
            Debug.Log("Selected");
            selected = true;
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
            if(UIManager.singleton.playerParty.gold >= price) {
                markPurchased();
                //add to playerparty
                Character fixName = Instantiate(character, UIManager.singleton.playerParty.transform);
                //if this isn't done the Instantiated object's name will be characterName(clone) so we did this to remove the clone from the name
                fixName.name = character.name;

                //deduct from player gold
                UIManager.singleton.playerParty.gold -= price;

                //update display since the price would change after a purchase
                UIManager.singleton.shopScreen.closeCharacters();
                UIManager.singleton.shopScreen.displayCharacters();

                //save character in map since shop is so far only available in maps
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
        UIManager.singleton.shopScreen.shop.characterPurchased[index] = true;
        //change color of display
    }
}
