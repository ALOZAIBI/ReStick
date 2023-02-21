using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAbilityDisplay : AbilityDisplay
{
    public Button button;
    public InventoryScreen inventoryScreen;
    private void Start() {
        inventoryScreen = GameObject.FindGameObjectWithTag("InventoryScreen").GetComponent<InventoryScreen>();
        button.onClick.AddListener(selectAbility);
    }

    private void selectAbility() {
        //if character had already been selected I.E character is selected first
        //when clicked set the inventorySCreen's ability selected to this. and prompt to confirm
        if(inventoryScreen.characterSelected != null) {
            inventoryScreen.abilitySelected = ability;
            //prompt to confirm
            inventoryScreen.inventoryCharacterScreen.addAbilityBtn.gameObject.SetActive(false);
            inventoryScreen.inventoryCharacterScreen.confirmAddAbilityBtn.gameObject.SetActive(true);
        }
        //if character hadn't been selected. I.E ability is selected first
        else if(inventoryScreen.characterSelected == null) {

        }
            
    }
}
