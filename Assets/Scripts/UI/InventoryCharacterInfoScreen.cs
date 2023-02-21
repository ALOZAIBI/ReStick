using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCharacterInfoScreen : CharacterInfoScreen
{
    //to be instantiated this is a different gameobject than the regular abilityDisplay because this one will have diff color
    public GameObject inventoryAbilityDisplay;

    public InventoryScreen inventoryScreen;

    public Button addAbilityBtn;
    public Button confirmAddAbilityBtn;
    private void Start() {
        addAbilityBtn.onClick.AddListener(addAbility);
        confirmAddAbilityBtn.onClick.AddListener(confirmAddAbility);
    }
    //displays the abilities in inventory when clicked
    private void addAbility() {
        //to destroy all abilityDisplayElements
        close();
        //then display inventory abilities
        displayInventoryAbilities();
    }

    private void confirmAddAbility() {
        //adds the ability to Character
        inventoryScreen.characterSelected.abilities.Add(inventoryScreen.abilitySelected);
        //adds ability to activeAbilities in playermanager
        //Debug.Log(inventoryScreen.playerParty.activeAbilities.name);
        inventoryScreen.abilitySelected.gameObject.transform.parent = inventoryScreen.playerParty.activeAbilities.transform;  
        //then goes back
        inventoryScreen.backToScreen();
        //then reverts to display add ability and not confirm add ability
        addAbilityBtn.gameObject.SetActive(true);
        confirmAddAbilityBtn.gameObject.SetActive(false);
    }

    public void displayInventoryAbilities() {
        //loops thorugh the abilities in abilityInventory
        foreach (Transform child in inventoryScreen.playerParty.abilityInventory.transform) {
            Ability ability = child.GetComponent<Ability>();
            GameObject temp = Instantiate(inventoryAbilityDisplay);
            //sets the instantiated object as child
            temp.transform.parent = abilityDisplayPanel.transform;
            AbilityDisplay displayTemp = temp.GetComponent<AbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            displayTemp.ability = ability;
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
