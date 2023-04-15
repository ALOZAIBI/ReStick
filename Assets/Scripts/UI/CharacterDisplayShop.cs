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

    public bool selected;
    public bool purchased;    
    private void Start() {
        //sets the image
        characerPortrait.sprite = character.GetComponent<SpriteRenderer>().sprite;
        characerPortrait.color = character.GetComponent<SpriteRenderer>().color;
        //sets the HPbar
        healthBar.character = character;
        //sets the name
        name.text = character.name;

        self.onClick.AddListener(select);
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
            markPurchased();
            //add to playerparty
            Character fixName =Instantiate(character, UIManager.singleton.playerParty.transform);
            //if this isn't done the Instantiated object's name will be characterName(clone) so we did this to remove the clone from the name
            fixName.name = character.name;
            
            SaveSystem.characterNumber = 0;
            //save character in map since shop is so far only available in maps
            foreach (Transform child in UIManager.singleton.playerParty.transform) {
                if (child.tag == "Character") {
                    Character temp = child.GetComponent<Character>();
                    SaveSystem.saveCharacterInMap(temp);
                }
            }
            //save shop PurchaseInfo
            SaveSystem.saveShopAbilitiesAndPurchaseInfo(UIManager.singleton.shopScreen.shop);
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
