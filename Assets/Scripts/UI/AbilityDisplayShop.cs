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
    public Image forColorPurposes;

    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;

    public Button self;
    private bool purchased;

    private void Start() {
        self.onClick.AddListener(select);
        forColorPurposes = GetComponent<Image>();
        forColorPurposes.color = new Color(0.7f, 0.7f, 0.7f);
    }

    public void highlight() {
        forColorPurposes.color = new Color(0.7f, 1, 0.7f);
    }
    public void unHighlight() {
        forColorPurposes.color = new Color(0.7f, 0.7f, 0.7f);
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
                markPurchased();
            //add to inventroy
            Instantiate(ability, UIManager.singleton.playerParty.abilityInventory.transform);
            //Since Shops are only in maps we save to map
            SaveSystem.saveInventoryInMap();
            }

        
    }
    private void markPurchased() {
        purchased = true;
        //change color of display
    }
}
