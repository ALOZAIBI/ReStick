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
    public Image confirmAddAbilityBtnImage;

    //pageindex 3 = prompt to add ability
    //pageindex 4 = confirm ability adding

    public void addAbilityPage() {
        addAbilityBtn.gameObject.SetActive(true);
        confirmAddAbilityBtn.gameObject.SetActive(false);
        pageIndex = 3;
    }

    public void confirmAddAbilityPage() {
        addAbilityBtn.gameObject.SetActive(false);
        confirmAddAbilityBtn.gameObject.SetActive(true);
        pageIndex = 4;
    }
    private void Start() {
        base.Start();
        addAbilityBtn.onClick.AddListener(addAbility);
        confirmAddAbilityBtn.onClick.AddListener(confirmAddAbility);
        confirmAddAbilityBtnImage = confirmAddAbilityBtn.GetComponent<Image>();
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
        //sets the ability's character to this character
        character.initRoundStart();
        //adds ability to activeAbilities in playermanager
        //Debug.Log(inventoryScreen.playerParty.activeAbilities.name);
        inventoryScreen.abilitySelected.gameObject.transform.parent = inventoryScreen.playerParty.activeAbilities.transform;
        //if ability was selected first go back to inventory screen landing page
        if (inventoryScreen.pageIndex == 2) {
            inventoryScreen.openLandingPage();
        }
        else {
            //else if character was selected first delete the inventory ablity displays then
            close();
            //update the character's ability display and 
            displayCharacterAbilities(inventoryScreen.characterSelected);
        }
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
            InventoryAbilityDisplay displayTemp = temp.GetComponent<InventoryAbilityDisplay>();
            //sets the displays name and description
            displayTemp.abilityName.text = ability.abilityName;
            displayTemp.description.text = ability.description;
            displayTemp.ability = ability;
            displayTemp.glow = true;
            //resetting scale to 1 cuz for somereaosn the scale is 167 otherwise
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Update() {
        //to make the button glow in and out for emphasis
        float x = 0.5f+Mathf.PingPong(Time.unscaledTime*0.5f,0.7f);
        confirmAddAbilityBtnImage.color = new Color(x,x ,x );
    }
}
